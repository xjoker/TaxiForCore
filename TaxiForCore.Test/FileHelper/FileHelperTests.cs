using System;
using System.IO;
using System.Threading;
using TaxiForCore.StringHelper;
using Xunit;

namespace TaxiForCore.Test.FileHelper
{
    public class FileHelperTests
    {
        string a = "233";
        string path = Path.Combine(AppContext.BaseDirectory, "233Test.txt");

        [Fact]
        public void ReadFileTest()
        {
            a.WriteToFile(path, true);
            var b = TaxiForCore.FileHelper.FileHelper.ReadFile(path);
            Assert.True(a == b);
        }

        [Fact]
        public void ReadFileForAllTest()
        {
            TaxiForCore.FileHelper.FileHelper.WriteFile(path, a);
            var b = TaxiForCore.FileHelper.FileHelper.ReadFileForAll(path);
            b.Position = 0;
            var R = new StreamReader(b);
            string myStr = R.ReadToEnd();
            Assert.True(a == myStr);
        }

        [Fact]
        public void WriteFileTest()
        {
            TaxiForCore.FileHelper.FileHelper.WriteFile(path, a);
            var b = TaxiForCore.FileHelper.FileHelper.ReadFile(path);
            Assert.True(a == b);
            Thread.Sleep(1000);
            TaxiForCore.FileHelper.FileHelper.WriteFile(path, "666", true);
            var c = TaxiForCore.FileHelper.FileHelper.ReadFile(path);
            Assert.True(a + "666" == c);

        }

    }
}
