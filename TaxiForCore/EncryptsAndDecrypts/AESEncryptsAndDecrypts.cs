using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TaxiForCore.EncryptsAndDecrypts
{
    public static class AESEncryptsAndDecrypts
    {
        private static byte[] staticIV = new byte[] { 122, 120, 99, 118, 98, 110, 109, 100, 102, 114, 97, 115, 100, 102, 103, 104 };

        /// <summary>
        /// 简单AES加密 可使用任意长度密钥
        /// </summary>
        /// <param name="text">明文</param>
        /// <param name="SecretKey">Key</param>
        /// <returns></returns>
        public static string SimpleEncrypt(this string text,string SecretKey)
        {
            string md5SecretKey = MD5Hash.GetMd5Hash(SecretKey);
            return Encrypt(text, md5SecretKey, staticIV);
        }

        /// <summary>
        /// 简单AES解密 只可以解密由 SimpleEncrypt 加密的
        /// </summary>
        /// <param name="text">明文</param>
        /// <param name="SecretKey">Key</param>
        /// <returns></returns>
        public static string SimpleDecrypt(this string text, string SecretKey)
        {
            string md5SecretKey = MD5Hash.GetMd5Hash(SecretKey);
            return Decrypt(text, md5SecretKey, staticIV);
        }


        /// <summary>
        /// AES 加密模块
        /// </summary>
        /// <param name="text">明文</param>
        /// <param name="SecretKey">Key</param>
        /// <param name="IV">IV</param>
        /// <returns>Base64编码的String</returns>
        public static string Encrypt(this string text, string SecretKey, Byte[] IV)
        {
            var key = Encoding.UTF8.GetBytes(SecretKey);

            using (var aesAlg = Aes.Create())
            {
                byte[] encrypted;
                aesAlg.Key = key;
                aesAlg.IV = IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            
                            swEncrypt.Write(text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }

                return Convert.ToBase64String(encrypted);
            }
        }

        /// <summary>
        /// AES 解密模块
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="SecretKey">Key</param>
        /// <param name="IV">IV</param>
        /// <returns>UTF-8编码的String</returns>
        public static string Decrypt(string cipherText, string SecretKey,Byte[] IV)
        {
            string plaintext = null;
            var fullCipher = Convert.FromBase64String(cipherText);
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(SecretKey);
                aesAlg.IV = IV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(fullCipher))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting  stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return plaintext;
            }
        }
    }
}
