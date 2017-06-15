using System.Security.Cryptography;
using TaxiForCore.EncryptsAndDecrypts;
using Xunit;

namespace TaxiForCore.Test.EncryptsAndDecrypts
{
    public class AESEncryptsAndDecryptsTests
    {

        [Fact]
        public void EncryptTest()
        {
            using (Aes myAes = Aes.Create())
            {
                var b = AESEncryptsAndDecrypts.Encrypt("TestEncrypt", "dofkrfaosrdedofkrfaosrdedofkrfao",myAes.IV);
                var c = AESEncryptsAndDecrypts.Decrypt(b, "dofkrfaosrdedofkrfaosrdedofkrfao", myAes.IV);
                Assert.Equal(c, "TestEncrypt");
            }
        }

        [Fact]
        public void SimpleEncryptTest()
        {
            var b = AESEncryptsAndDecrypts.SimpleEncrypt("TestEncrypt", "123456");
            var c = AESEncryptsAndDecrypts.SimpleDecrypt(b, "123456");
            Assert.Equal(c, "TestEncrypt");
        }
    }
}
