using System.Security.Cryptography;

namespace SamsWarehouseApplication.Services
{
    public class EncryptionService
    {
        private readonly string _secretKey;

        public EncryptionService(IConfiguration configuration)
        {
            _secretKey = configuration["SecretKey"];
        }

        public byte[] EncryptByteArray(byte[] fileData)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(_secretKey);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, 16);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(fileData, 0, fileData.Length);
                        csEncrypt.FlushFinalBlock();
                        return msEncrypt.ToArray();
                    }    
                }
            }
        }

        public byte[] DecryptByteArray(byte[] encryptedFileData)
        {
            using (AesManaged aesAlgo = new AesManaged())
            {
                aesAlgo.Key = System.Text.Encoding.UTF8.GetBytes(_secretKey);

                byte[] IV = new byte[16];
                Array.Copy(encryptedFileData, 0, IV, 0, 16);

                ICryptoTransform decryptor = aesAlgo.CreateDecryptor(aesAlgo.Key, IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedFileData)) 
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(encryptedFileData, 16, encryptedFileData.Length - 16);
                        csDecrypt.FlushFinalBlock();
                        return msDecrypt.ToArray();
                    }
                }
            }
        }
    }
}
