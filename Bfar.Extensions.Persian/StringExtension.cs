using System;
using System.Globalization;

namespace Bfar.Extensions.Persian
{
    public static class StringExtension
    {
        public static DateTime ToGregorianDateTime(this string data)
        {
            //PersianCalendar pc = new PersianCalendar();
            var parts = data.Split(new string[] { "-", " ", ":", "/" }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 3 ? new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), 0, 0, 0, new PersianCalendar())
                : new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]), new PersianCalendar());
            //var persian = DateTime.Parse(data);
            //return pc.ToDateTime(persian.Year, persian.Month, persian.Day, persian.Hour, persian.Minute, persian.Second, persian.Millisecond);
        }
        public static string ToGregorianDateTimeString(this string data)
        {
            return ToGregorianDateTime(data).ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string ToGregorianDateString(this string data)
        {
            return ToGregorianDateTime(data).ToString("yyyy-MM-dd");
        }

        public static string ToEnglishNumber(this string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(persian[j], j.ToString());

            return input;
        }
    }
}
