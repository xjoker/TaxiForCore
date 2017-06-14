using System.Collections.Generic;
using System.Linq;
using TaxiForCore.Dictionary;
using Xunit;

namespace TaxiForCore.Test.Dictionary
{
    public class DictionaryTests
    {
        private Dictionary<int, string> a = new Dictionary<int, string>() { [1] = "q", [2] = "w" };
        private Dictionary<int, string> b = new Dictionary<int, string>() { [3] = "z", [4] = "x" };
        private Dictionary<int, string> c = new Dictionary<int, string>() { [1] = "z", [4] = "x" };

        private Dictionary<int, string> d = new Dictionary<int, string>() { [1] = "q", [2] = "w", [3] = "z", [4] = "x" };
        private Dictionary<int, string> e = new Dictionary<int, string>() { [1] = "z", [2] = "w", [4] = "x" };

        [Fact]
        public void MergeDictionaryAddTest()
        {
            Assert.True(DictionaryHelper.MergeDictionaryAdd(a, b).SequenceEqual(d));
        }

        [Fact]
        public void MergeDictionaryReplaceTest()
        {
            Assert.True(DictionaryHelper.MergeDictionaryReplace(a, c).SequenceEqual(e));
        }

        [Fact]
        public void GetValueTest()
        {
            var aa = a.GetValue(1);
            Assert.True(aa == "q");
        }

        [Fact]
        public void AddRangeTest()
        {
            var aa = a.AddRange(b, true);
            Assert.True(aa.SequenceEqual(d));
        }
    }
}
