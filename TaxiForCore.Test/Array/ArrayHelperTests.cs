using System;
using System.Collections.Generic;
using System.Text;
using TaxiForCore.Array;
using Xunit;

namespace TaxiForCore.Test.Array
{
    public class ArrayHelperTests
    {
        [Fact]
        public void JoinToStringTest()
        {
            int[] a = new int[5] { 1, 2, 3, 4, 5 };
            var b = a.JoinToString(",");
            Assert.Equal(b, "1,2,3,4,5");
        }
    }
}
