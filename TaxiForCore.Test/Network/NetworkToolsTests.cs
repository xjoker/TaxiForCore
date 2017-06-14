using System;
using System.Collections.Generic;
using System.Text;
using TaxiForCore.Network;
using Xunit;

namespace TaxiForCore.Test.Network
{
    public class NetworkToolsTests
    {
        [Fact]
        public void PingCheckDetailedTest()
        {
            var b = NetworkTools.PingCheckDetailed("xjoker.us");
            Assert.False(b == new PingResponseType());
        }

        [Fact]
        public void PingCheckTest()
        {
            var b = NetworkTools.PingCheck("baidu.com");
            Assert.False(b == false);
        }

        [Fact]
        public void PingDelayTest()
        {
            var b = NetworkTools.PingDelay("baidu.com");
            Assert.False(b == null);
        }

        [Fact]
        public void GetLocalIPsTest()
        {
            var b = NetworkTools.GetLocalIPs();
            Assert.False(b == null);
        }

        [Fact]
        public void GetLocalMacsTest()
        {
            var b = NetworkTools.GetLocalMacs();
            Assert.False(b == null);
        }

        [Fact]
        public void GetAllUsePortTest()
        {
            var b = NetworkTools.GetAllUsePort();
            Assert.False(b == null);
        }
    }
}
