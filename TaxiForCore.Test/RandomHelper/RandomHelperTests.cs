using Xunit;

namespace TaxiForCore.Test.RandomHelper
{
    public class RandomHelperTests
    {
        [Fact]
        public void GenerateRandomPasswordTest()
        {
            var b = TaxiForCore.RandomHelper.RandomHelper.GenerateRandomPassword(10, includeNumber: false, includeMixedCase: false, includePunctuation: false);
            Assert.NotNull(b);
        }


    }
}
