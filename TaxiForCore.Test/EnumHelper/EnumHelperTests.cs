using System.Collections.Generic;
using TaxiForCore.Dictionary;
using TaxiForCore.EnumHelper;
using TaxiForCore.List;
using Xunit;

namespace TaxiForCore.Test.EnumHelper
{
    public class EnumHelperTests
    {
        enum TestEnum
        {
            [System.ComponentModel.Description("AAA")]
            A = 1,
            [System.ComponentModel.Description("BBB")]
            B = 2,
            [System.ComponentModel.Description("CCC")]
            C = 3
        }


        [Fact]
        public void ToEnumTest()
        {
            
            var b = TaxiForCore.EnumHelper.EnumHelper.ToEnum<TestEnum>("A");
            Assert.True(b == TestEnum.A);
        }

        [Fact]
        public void ToEnumNameTest()
        {
            var b = new List<string> { "A", "B", "C" };
            var d = TaxiForCore.EnumHelper.EnumHelper.ToEnumName<TestEnum>();
            Assert.True(b.Compare(d));
        }

        [Fact]
        public void GetIntValueTest()
        {
            var b = TestEnum.B.GetIntValue();
            Assert.True(b == 2);
        }

        [Fact]
        public void GetDescriptionTest()
        {
            var b = TestEnum.B.GetDescription();
            Assert.True(b == "BBB");
        }

        [Fact]
        public void ToDictionaryTest()
        {
            var d = new Dictionary<int, string>()
            {
                {1,"AAA" },
                {2,"BBB" },
                {3,"CCC" }
            };
            var b = TaxiForCore.EnumHelper.EnumHelper.ToDictionary<TestEnum>();
            Assert.True(d.DictionaryEqual(b));
        }

    }
}
