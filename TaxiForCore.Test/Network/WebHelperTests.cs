using System;
using System.Collections.Generic;
using System.Text;
using TaxiForCore.Network;
using Xunit;


namespace TaxiForCore.Test.Network
{
    public class WebHelperTests
    {
        [Fact]
        public void GetAsyncTest()
        {
            var b = WebHelper.GetAsync("https://ip.sb");
            Assert.False(b.Result==null);
        }

        [Fact]
        public void PostAsyncTest()
        {
            var b = WebHelper.PostAsync("http://xjokertestapi001.azurewebsites.net/api/values","666");
            Assert.False(b.Result == null);
        }
    }
}
