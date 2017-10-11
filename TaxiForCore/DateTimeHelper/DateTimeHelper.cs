using System;

namespace TaxiForCore.DateTimeHelper
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 将UNIX时间戳转为时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(this string timeStamp)
        {
            // .NET Version >= 4.6 can use
            long lTime = (long)Convert.ToDouble(timeStamp);
            return DateTimeOffset.FromUnixTimeSeconds(lTime).LocalDateTime;
        }

        /// <summary>
        /// 将DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int DateTimeToStamp(this DateTime time)
        {
            // https://msdn.microsoft.com/en-us/library/system.datetimeoffset.tounixtimeseconds(v=vs.110).aspx
            // .NET Version >= 4.6 can use
            return (Int32)new DateTimeOffset(time).ToUnixTimeSeconds();
        }

        /// <summary>  
        /// 取得某月的第一天  
        /// </summary>  
        /// <param name="datetime">要取得月份第一天的时间</param>  
        /// <returns></returns>  
        public static DateTime FirstDayOfMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        /// <summary>
        /// 取得某月的最后一天  
        /// </summary>  
        /// <param name="datetime">要取得月份最后一天的时间</param>  
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        /// <summary>  
        /// 取得上个月第一天  
        /// </summary>  
        /// <param name="datetime">要取得上个月第一天的当前时间</param>  
        /// <returns></returns> 
        public static DateTime FirstDayOfPreviousMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(-1);
        }

        /// <summary>  
        /// 取得上个月的最后一天  
        /// </summary>  
        /// <param name="datetime">要取得上个月最后一天的当前时间</param>  
        /// <returns></returns>  
        public static DateTime LastDayOfPrdviousMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddDays(-1);
        }

        /// <summary>
        /// 获取年份的最后一天
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime LastDayOfYear(int year)
        {
            return LastDayOfMonth(new DateTime(year, 12, 1));
        }

        /// <summary>
        /// 获取星期X 描述(英文)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetWeekDayEnglish(DateTime date)
        {
            return (date.DayOfWeek.ToString());
        }

        /// <summary>
        /// 获取星期X 描述(中文)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetWeekDayChinese(DateTime date)
        {
            string[] weekDay = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return weekDay[(int)(date.DayOfWeek)];
        }

        /// <summary>
        /// 格式化日期(格式yyyy-MM-dd)
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <returns></returns>
        public static string FormatDate(DateTime? datetime)
        {
            if (datetime.HasValue)
            {
                return FormatDate(datetime.Value);
            }
            return string.Empty;
        }

        /// <summary>
        /// 格式化日期(格式 yyyy-MM-dd HH:mm:ss)
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <returns></returns>
        public static string FormatDateHasMilliSecond(DateTime? datetime)
        {
            return FormatDate(datetime, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="datetime">日期</param>
        /// <param name="format">格式字符串</param>
        /// <returns></returns>
        public static string FormatDate(DateTime? datetime, string format)
        {
            if (datetime.HasValue)
            {
                if (datetime.Value == DateTime.MinValue || datetime.Value == DateTime.MaxValue)
                {
                    return string.Empty;
                }
                return datetime.Value.ToString(format);
            }
            return string.Empty;
        }

        /// <summary>
        /// 判断年份是否为闰年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }

        /// <summary>
        /// 判断日期是否为周末
        /// 可选是否不判断周六
        /// </summary>
        /// <param name="day"></param>
        /// <param name="notIncludingSaturday"></param>
        /// <returns></returns>
        public static bool IsWeekEnd(DateTime day,bool notIncludingSaturday=false)
        {
            if (notIncludingSaturday)
            {
                return day.DayOfWeek == DayOfWeek.Sunday;
            }

            return day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
