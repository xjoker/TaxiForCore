using System;
using System.Collections.Generic;
using System.Text;
using TaxiForCore.DateTimeHelper;
using TaxiForCore.StringHelper;
using Xunit;

namespace TaxiForCore.Test.DateTimeHelperTests
{
    public class DateTimeHelperTests
    {
        private string a = "1483346453";
        private DateTime dt = Convert.ToDateTime("2017/1/2 16:40:53");

        [Fact]
        public void StampToDateTimeTest()
        {
            var c = a.StampToDateTime();
            Assert.Equal(dt, c);
        }

        [Fact]
        public void DateTimeToStampTest()
        {

            int c = dt.DateTimeToStamp();
            Assert.Equal(a.ToInt(), c);
        }

        [Fact]
        public void FirstDayOfMonthTest()
        {
            var aa = DateTimeHelper.DateTimeHelper.FirstDayOfMonth(dt);
            DateTime bb = Convert.ToDateTime("2017/1/1 16:40:53");
            Assert.Equal(aa, bb);
        }

        [Fact]
        public void LastDayOfMonthTest()
        {
            var aa = DateTimeHelper.DateTimeHelper.LastDayOfMonth(dt);
            DateTime bb = Convert.ToDateTime("2017/1/31 16:40:53");
            Assert.Equal(aa, bb);
        }

        [Fact]
        public void FirstDayOfPreviousMonthTest()
        {
            var aa = DateTimeHelper.DateTimeHelper.FirstDayOfPreviousMonth(dt);
            DateTime bb = Convert.ToDateTime("2016/12/1 16:40:53");
            Assert.Equal(aa, bb);
        }

        [Fact]
        public void LastDayOfPrdviousMonthTest()
        {
            var aa = DateTimeHelper.DateTimeHelper.LastDayOfPrdviousMonth(dt);
            DateTime bb = Convert.ToDateTime("2016/12/31 16:40:53");
            Assert.Equal(aa, bb);
        }

        [Fact]
        public void GetWeekDayTest()
        {
            var aa = new DateTime(2017, 5, 20);
            var dd = DateTimeHelper.DateTimeHelper.GetWeekDayEnglish(aa);
            Assert.Equal("Saturday", dd);
        }
    }
}
