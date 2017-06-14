using TaxiForCore.EncryptsAndDecrypts;
using Xunit;

namespace TaxiForCore.Test.EncryptsAndDecrypts
{
    public class AESEncryptsAndDecryptsTests
    {

        [Fact]
        public void EncryptTest()
        { 
            var b = AESEncryptsAndDecrypts.Encrypt("TestEncrypt", "dofkrfaosrdedofkrfaosrdedofkrfao");
            var c = AESEncryptsAndDecrypts.Decrypt(b, "dofkrfaosrdedofkrfaosrdedofkrfao");
            Assert.Equal(c, "TestEncrypt");
        }

        [Fact]
        public void DecryptTest()
        {
            var b = AESEncryptsAndDecrypts.Decrypt("a8FRqc+qOoi3rV0cvfjlpuZoL9K7E/DfsRZ2nd1fS0c=", "dofkrfaosrdedofkrfaosrdedofkrfao");
            string check = "TestEncrypt";
            Assert.Equal(b, check);
        }
    }
}
