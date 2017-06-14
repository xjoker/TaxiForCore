using System;
using System.Security.Cryptography;
using TaxiForCore.Array;

namespace TaxiForCore.EncryptsAndDecrypts
{
    public static class PasswordHash
    {
        public const int SALT_BYTES = 24;

        public const int HASH_BYTES = 36;

        public const int PBKDF2_ITERATIONS = 20;

        private const int ITERATION_INDEX = 1;

        private const int SALT_INDEX = 2;

        private const int PBKDF2_INDEX = 3;

        /// <summary>
        /// 密码加密方法
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CreateHash(string password)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] array = new byte[SALT_BYTES];
                rng.GetBytes(array);
                byte[] inArray = PBKDF2(password, array, PBKDF2_ITERATIONS, HASH_BYTES);
                return string.Concat(new object[]
                {
                "sha1:",
                PBKDF2_ITERATIONS,
                ":",
                Convert.ToBase64String(array),
                ":",
                Convert.ToBase64String(inArray)
                });
            }
        }


        /// <summary>
        /// 校验密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="goodHash"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string password, string goodHash)
        {
            string[] array = goodHash.Split(new char[] { ':' });
            int iterations = int.Parse(array[ITERATION_INDEX]);
            byte[] salt = Convert.FromBase64String(array[SALT_INDEX]);
            byte[] array2 = Convert.FromBase64String(array[PBKDF2_INDEX]);
            byte[] b = PBKDF2(password, salt, iterations, array2.Length);
            return ArrayHelper.SlowEquals(array2, b);
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            return new Rfc2898DeriveBytes(password, salt)
            {
                IterationCount = iterations
            }.GetBytes(outputBytes);
        }
    }
}
