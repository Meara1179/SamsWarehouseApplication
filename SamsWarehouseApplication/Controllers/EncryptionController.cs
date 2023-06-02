using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamsWarehouseApplication.Services;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SamsWarehouseApplication.Controllers
{
    public class EncryptionController : Controller
    {
        private readonly ILogger<EncryptionController> _logger;
        private readonly FileUploaderService _uploader;

        public EncryptionController(ILogger<EncryptionController> logger, FileUploaderService uploader)
        {
            _logger = logger;
            _uploader = uploader;
        }

        /// <summary>
        /// Returns the Encryption/Index.cshtml View.
        /// </summary>
        /// <returns>View</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Takes the user supplied image and passing it through an encryption function before saving it to storage.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            var validate = ValidateFileUpload(file);

            if (validate.Count > 0)
            {
                foreach (var error in validate)
                {
                    Console.WriteLine(error.ToString());
                    ModelState.AddModelError("Upload Error", error);
                }
                return View("Index");
            }

            await _uploader.SaveFile(file);
            return View("Index");
        }

        /// <summary>
        /// Validates that the image that is being uploaded is smaller than 10MB and is one of the approved file extensions.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private List<string> ValidateFileUpload(IFormFile file)
        {
            List<string> errors = new List<string>();

            if (file.Length > 10000000)
            {
                errors.Add("File must be smaller than 10MB");
            }

            if (file.FileName.Contains('.'))
            {
                string[] acceptableExtensions = { "png", "bmp", "jpg", "jpeg", "gif" };
                string extension = file.FileName.Split('.').LastOrDefault();
                if (extension == null)
                {
                    errors.Add("File does not have acceptable extension.");
                }    
                else
                {
                    if (!acceptableExtensions.Any(c => c.Equals(extension)))
                    {
                        errors.Add($"{extension} is not allowed");
                    }
                }
            }
            return errors;
        }

        /// <summary>
        /// Loads an image that is stored on the server and displays it on the page.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LoadImage(string fileName)
        {
            byte[] fileBytes = await _uploader.ReadFileIntoMemory(fileName);
            var imageData = System.Convert.ToBase64String(fileBytes);
            string extension = fileName.Split('.').LastOrDefault();

            ViewData["ImageSource"] = $"data:image/{fileName};base64,{imageData}";
            ViewData["ImageAlt"] = "Image Loaded";
            return View("Index");
        }

        /// <summary>
        /// Sends the selected image to the user to download from the server's local storage.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            byte[] fileBytes = await _uploader.ReadFileIntoMemory(fileName);

            if (fileBytes == null || fileBytes.Length == 0) 
            {
                return RedirectToAction(nameof(Index));
            }

            return File(fileBytes, "application/octet-stream", fileDownloadName: fileName);
        }
    }
}
