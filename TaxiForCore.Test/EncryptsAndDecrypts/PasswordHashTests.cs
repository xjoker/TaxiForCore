using TaxiForCore.EncryptsAndDecrypts;
using Xunit;

namespace TaxiForCore.Test.EncryptsAndDecrypts
{
    public class PasswordHashTests
    {
        [Fact]
        public void CreateHashTest()
        {
            var b = PasswordHash.CreateHash("123456");
            var c = PasswordHash.ValidatePassword("123456", b);
            Assert.True(c);
        }
    }
}
