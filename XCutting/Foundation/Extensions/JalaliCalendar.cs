namespace Bfar.XCutting.Foundation.Extensions
{
    public static class JalaliCalendar
    {
        private static string[] dayNames = { "شنبه", "یک شنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنج شنبه", "جمعه" };
        private static string[] monthNames = { string.Empty, "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
        private static int[] solarMonthDays = { 0, 31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 30 };
        private static int[] gregMonthDays = { 29, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private static int diffDays = 226899;
        public static string[] DayNames { get { return dayNames; } }
        public static string[] MonthNames { get { return monthNames; } }
        public static int[] SolarMonthDays { get { return solarMonthDays; } }
        public static int[] GregorianMonthDays { get { return gregMonthDays; } }
        public static int DiffDays { get { return diffDays; } }

        public static int[] GregorianToJalali(int year, int month, int day)
        {
            int[] g_d_m = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            int gy2 = (month > 2) ? (year + 1) : year;
            int days = 355666 + (365 * year) + ((int)((gy2 + 3) / 4)) - ((int)((gy2 + 99) / 100)) + ((int)((gy2 + 399) / 400)) + day + g_d_m[month - 1];
            int jy = -1595 + (33 * ((int)(days / 12053)));
            days %= 12053;
            jy += 4 * ((int)(days / 1461));
            days %= 1461;
            if (days > 365)
            {
                jy += (int)((days - 1) / 365);
                days = (days - 1) % 365;
            }
            int jm = (days < 186) ? 1 + (int)(days / 31) : 7 + (int)((days - 186) / 30);
            int jd = 1 + ((days < 186) ? (days % 31) : ((days - 186) % 30));
            int[] jalali = { jy, jm, jd };
            return jalali;
        }
        public static int[] JalaliToGregorian(int year, int month, int day)
        {
            year += 1595;
            int days = -355668 + (365 * year) + (((int)(year / 33)) * 8) + ((int)(((year % 33) + 3) / 4)) + day + ((month < 7) ? (month - 1) * 31 : ((month - 7) * 30) + 186);
            int gy = 400 * ((int)(days / 146097));
            days %= 146097;
            if (days > 36524)
            {
                gy += 100 * ((int)(--days / 36524));
                days %= 36524;
                if (days >= 365) days++;
            }
            gy += 4 * ((int)(days / 1461));
            days %= 1461;
            if (days > 365)
            {
                gy += (int)((days - 1) / 365);
                days = (days - 1) % 365;
            }
            int gd = days + 1;
            int[] sal_a = { 0, 31, ((gy % 4 == 0 && gy % 100 != 0) || (gy % 400 == 0)) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int gm;
            for (gm = 0; gm < 13 && gd > sal_a[gm]; gm++) gd -= sal_a[gm];
            int[] gregorian = { gy, gm, gd };
            return gregorian;
        }


    }
}
