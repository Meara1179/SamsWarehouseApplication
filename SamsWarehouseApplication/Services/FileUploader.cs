using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SamsWarehouseApplication.Services
{
    public class FileUploaderService
    {
        string _uploadRootPath;
        private readonly EncryptionService _encryptionService;

        public FileUploaderService(IWebHostEnvironment env, EncryptionService encryptionService)
        {
            _uploadRootPath = Path.Combine(env.WebRootPath, "Uploads");
            _encryptionService = encryptionService;
        }

        public string UniqueFileName(string fileName)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(_uploadRootPath);

                if (!dir.EnumerateFiles().Any(c => c.Name.Equals(fileName)))
                {
                    return fileName;
                }

                string extension = fileName.Split('.').LastOrDefault();

                if(String.IsNullOrEmpty(extension))
                {
                    extension = "txt";
                }

                return $"{Guid.NewGuid()}.{extension}";

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task SaveFile(IFormFile file)
        {
            string fileName = UniqueFileName(file.FileName);

            byte[] fileContents;

            using (var stream = new MemoryStream()) 
            {
                await file.CopyToAsync(stream);
                fileContents = stream.ToArray();
            }

            var encryptedData = _encryptionService.EncryptByteArray(fileContents);

            using (var stream = new MemoryStream(encryptedData))
            {
                var targetFile = Path.Combine(_uploadRootPath, fileName);

                using (var fileStream = new FileStream(targetFile, FileMode.Create))
                {
                    stream.WriteTo(fileStream);
                }
            }

        }

        public FileInfo LoadFile(string fileName)
        {
            DirectoryInfo dir = new DirectoryInfo(_uploadRootPath);

            if (!dir.EnumerateFiles().Any(c => c.Name.Equals(fileName)))
            {
                return null;
            }

            return dir.EnumerateFiles().Where(c => c.Name.Equals(fileName)).FirstOrDefault();            
        }

        public string GetFileExtension(string fileName)
        {
            if(new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType))
            {
                return contentType;
            }

            return null;

        }

        public async Task<byte[]> ReadFileIntoMemory(string fileName)
        {

            var file = LoadFile(fileName);

            if (file == null)
            {
                return null;
            }

            using (var memStream = new MemoryStream())
            {
                using (var fileStream = File.OpenRead(file.FullName))
                {
                    fileStream.CopyTo(memStream);
                    var encryptedData = memStream.ToArray();

                    return _encryptionService.DecryptByteArray(encryptedData);
                }
            }
        }

        public async Task<string> GetFilePath(string fileName)
        {
            var file = LoadFile(fileName);

            if(file == null)
            {
                return null;
            }

            var directory = file.Directory.Name;

            if (directory.Equals("Uploads"))
            {
                return $"/{directory}/{file.Name}";
            }
            else
            {
                return $"/Uploads/{directory}/{file.Name}";
            }

        }
    }
}
