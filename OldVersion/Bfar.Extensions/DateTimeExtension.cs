using System;
using System.Globalization;

namespace Bfar.Extensions
{
    public static class DateTimeExtension
    {
        private static string longFormat = "yyyy-MM-dd HH:mm:ss";
        private static string shortFormat = "yyyy-MM-dd";
        public static string ToFullPersianString(this DateTime data)
        {
            PersianCalendar pc = new PersianCalendar();
            string k = string.Empty;
            try
            {
                k= string.Format($"{pc.GetYear(data)}-{pc.GetMonth(data)}-{pc.GetDayOfMonth(data)} {pc.GetHour(data)}:{pc.GetMinute(data)}:{pc.GetSecond(data)}");
            }
            catch (ArgumentOutOfRangeException)
            {
                k = DateTime.Now.ToString(longFormat);
            }
            return k;
            //return pc.ToDateTime(pc.GetYear(now), pc.GetMonth(now), pc.GetDayOfMonth(now), pc.GetHour(now), pc.GetMinute(now), pc.GetSecond(now),0);
            
        }
        public static string ToPersianDateString(this DateTime data)
        {
            PersianCalendar pc = new PersianCalendar();
            string k = string.Empty;
            try
            {
                k = string.Format($"{pc.GetYear(data)}-{pc.GetMonth(data)}-{pc.GetDayOfMonth(data)}");
            }
            catch (ArgumentOutOfRangeException)
            {
                k = DateTime.Now.ToString(shortFormat);
            }
            return k;
            //return pc.ToDateTime(pc.GetYear(now), pc.GetMonth(now), pc.GetDayOfMonth(now), pc.GetHour(now), pc.GetMinute(now), pc.GetSecond(now),0);

        }
        public static DateTime ToGregorianDateTime(this DateTime data)
        {
            if (data.Month == 7 && data.Day == 31)
                data=data.AddDays(1);
            return new PersianCalendar().ToDateTime(data.Year, data.Month, data.Day, data.Hour, data.Minute, data.Second, data.Millisecond);

        }
        public static string ToGregorianDateString(this DateTime data)
        {
            return data.ToString(shortFormat);

        }
        public static string ToStandardDateTimeString(this DateTime data)
        {
            return data.ToString(longFormat);

        }
        public static string ToStandardDateString(this DateTime data)
        {
            return data.ToString(shortFormat);

        }

        public static DateTime ToInvariantDateTime(this DateTime data)
        {
            var tempDate = data.ToString(longFormat, CultureInfo.InvariantCulture).Split(new string[] {":","-"," " },StringSplitOptions.RemoveEmptyEntries);
            if (tempDate[1] == "07" && tempDate[2] == "31")
            {
                tempDate[1] = "08";
                tempDate[2] = "01";
            }
            if (tempDate[1] == "08" && tempDate[2] == "31")
            {
                tempDate[1] = "09";
                tempDate[2] = "01";
            }
            if (tempDate[1] == "10" && tempDate[2] == "31")
            {
                tempDate[1] = "11";
                tempDate[2] = "01";
            }
            if (tempDate[1] == "12" && (tempDate[2] == "30" || tempDate[2] == "31"))
            {
                tempDate[1] = "12";
                tempDate[2] = "01";
            }
            return Convert.ToDateTime($"{tempDate[0]}-{tempDate[1]}-{tempDate[2]} {tempDate[3]}:{tempDate[4]}",CultureInfo.CurrentCulture);
        }
        public static string ToInvariantDateTimeString(this DateTime data)
        {
            return data.ToString(longFormat, CultureInfo.InvariantCulture);
        }
        public static string ToInvariantDateString(this DateTime data)
        {
            return data.ToString(shortFormat, CultureInfo.InvariantCulture);
        }
        public static DateTime InvariantNow(this DateTime data)
        {
            return Convert.ToDateTime(DateTime.Now.ToString(longFormat, CultureInfo.InvariantCulture));
        }
    }
}
