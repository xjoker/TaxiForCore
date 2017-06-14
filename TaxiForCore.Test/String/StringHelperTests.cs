using System;
using System.IO;
using TaxiForCore.StringHelper;
using Xunit;

namespace TaxiForCore.Test.String
{
    public class StringHelperTests
    {
        [Fact]
        public void WriteToFileTest()
        {
            string a = "233";
            string path = Path.Combine(AppContext.BaseDirectory, "233Test.txt");
            a.WriteToFile(path, true);
            bool b = File.Exists(path);
            Assert.True(b);
        }

        [Fact]
        public void ToIntTest()
        {
            string a = "1";
            int b = 1;
            Assert.True(a.ToInt() == b);
        }

        [Fact]
        public void IsIntTest()
        {
            string a = "1";
            Assert.True(a.IsInt());
        }


        [Fact]
        public void IsNullOrEmptyTest()
        {
            string a = null;
            Assert.True(a.IsNullOrEmpty());
        }

        [Fact]
        public void IsNullOrWhiteSpaceTest()
        {
            string a = "";
            Assert.True(a.IsNullOrWhiteSpace());
        }
    }
}
