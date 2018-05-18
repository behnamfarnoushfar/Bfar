using System;
using System.Globalization;

namespace Bfar.Extensions.Persian
{
    public static class DateTimeExtension
    {
        public static string ToFullPersianString(this DateTime data)
        {
            PersianCalendar pc = new PersianCalendar();
            string k = string.Empty;
            try
            {
                k = string.Format($"{pc.GetYear(data)}-{pc.GetMonth(data)}-{pc.GetDayOfMonth(data)} {pc.GetHour(data)}:{pc.GetMinute(data)}:{pc.GetSecond(data)}");
            }
            catch (ArgumentOutOfRangeException)
            {
                k = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
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
                k = DateTime.Now.ToString("yyyy-MM-dd");
            }
            return k;
            //return pc.ToDateTime(pc.GetYear(now), pc.GetMonth(now), pc.GetDayOfMonth(now), pc.GetHour(now), pc.GetMinute(now), pc.GetSecond(now),0);

        }
        public static DateTime ToGregorianDateTime(this DateTime data)
        {
            return new PersianCalendar().ToDateTime(data.Year, data.Month, data.Day, data.Hour, data.Minute, data.Second, data.Millisecond);

        }
        public static string ToGregorianDateString(this DateTime data)
        {
            return data.ToString("yyyy-MM-dd");

        }
        public static string ToStandardDateTimeString(this DateTime data)
        {
            return data.ToString("yyyy-MM-dd HH:mm:ss");

        }
    }
}
