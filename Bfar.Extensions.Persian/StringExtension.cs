using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bfar.Extensions.Persian
{
    public static class StringExtension
    {
        private static Regex mobileRegex = new Regex(@"^09\d{9}$");
        private static Regex mobileSystemRegex = new Regex(@"^989\d{9}$");
        private static string mcodeRegex = "^[0-9]{10,10}$";
        private static Regex regMCode = new Regex("^[0-9]{10,10}$");

        public static DateTime ToGregorianDateTime(this string data)
        {
            //PersianCalendar pc = new PersianCalendar();
            if (data.Length > 0)
            {
                var parts = data.Split(new string[] { "-", " ", ":","/" }, StringSplitOptions.RemoveEmptyEntries);
                if(parts[1]=="07" && parts[2]=="31")
                {
                    parts[1] = "8";
                    parts[2] = "1";
                }
                return parts.Length==3 ? new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), 0, 0, 0, new PersianCalendar())
                    :new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]), new PersianCalendar());
            }
            else return new DateTime();
            //var persian = DateTime.Parse(data);
            //return pc.ToDateTime(persian.Year, persian.Month, persian.Day, persian.Hour, persian.Minute, persian.Second, persian.Millisecond);
        }
        public static bool IsValidMobileNumber(this string input)
        {
            return mobileRegex.IsMatch(input);
        }
        public static bool IsValidMobileSystemNumber(this string input)
        {
            return mobileSystemRegex.IsMatch(input);
        }
        public static string ToGregorianDateTimeString(this string data)
        {
            return ToGregorianDateTime(data).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
        public static string ToGregorianDateString(this string data)
        {
            var oldCulture = SwitchCurrentCultureInfo("en");
            var result= ToGregorianDateTime(data).ToString("yyyy-MM-dd");
            SwitchCurrentCultureInfo(oldCulture);
            return result;
        }

        public static DateTime ToInvariantGregorianDateTime(this string data)
        {
            return Convert.ToDateTime(data, CultureInfo.InvariantCulture);
        }
        public static Dictionary<string,string> ToDictionary(this string data)
        {
            return data.Split('&')
              .Select(value => value.Split('='))
              .ToDictionary(pair => pair[0], pair => pair[1]);
        }

        public static string ToEnglishNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(persian[j],j.ToString() );

            return input;
        }
        public static string ToNormalPersianString(this string input)
        {
            return string.IsNullOrEmpty(input)?input: input.Replace('ي', 'ی').Replace('ئ', 'ی').Replace('أ', 'ا').Replace('إ', 'ا')
                .Replace('ء', 'ی').Replace('ؤ', 'و').Replace('ۀ', 'ه').Replace('ة', 'ه').Replace('ك', 'ک')
                .Replace('ً', 'ک').Replace("  ", " ").Trim();
        }
        public static string ToStandardMobileNumber(this string input)
        {
            return string.Format("98{0}", input[0] == '0' ? input.Substring(1) : input);
        }
        private static CultureInfo SwitchCurrentCultureInfo(CultureInfo culture)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture.Name);
            return culture;
        }
        public static bool IsValidNationalCode(this string NationalCode)
        {

            if (!regMCode.IsMatch(NationalCode))
            {
                return false;
            }
            NationalCode = NationalCode + "+";
            char[] array = NationalCode.ToCharArray();
            Array.Reverse(array);
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            for (int i = 2; i <= 10; i++)
            {
                num += Convert.ToInt32(array[i].ToString()) * i;
            }
            num2 = num % 0xb;
            if (num2 < 2)
            {
                num3 = num2;
            }
            else
            {
                num3 = 11 - num2;
            }
            return (num3 == Convert.ToInt32(array[1].ToString()));
        }
        private static CultureInfo SwitchCurrentCultureInfo(string culture)
        {
            var currentUI = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var cultureInfo = new CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
            return currentUI;
        }
    }
}
