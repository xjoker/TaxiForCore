using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TaxiForCore.EncryptsAndDecrypts
{
    public static class AESEncryptsAndDecrypts
    {

        /// <summary>
        /// AES 加密模块
        /// </summary>
        /// <param name="text">明文</param>
        /// <param name="SecretKey">Key</param>
        /// <param name="ivString">IV</param>
        /// <returns>Base64编码的String</returns>
        public static string Encrypt(this string text, string SecretKey)
        {
            var key = Encoding.UTF8.GetBytes(SecretKey);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        /// <summary>
        /// AES 解密模块
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="SecretKey">Key</param>
        /// <param name="ivString">IV</param>
        /// <returns>UTF-8编码的String</returns>
        public static string Decrypt(string cipherText, string SecretKey)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(SecretKey);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}
