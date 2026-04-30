using Bfar.XCutting.Abstractions.Decorators;
using Bfar.XCutting.Foundation.Constants;
using System.Globalization;

namespace Bfar.XCutting.Foundation.Adapters
{
    public sealed class DateTimeAdapter : IDateTimeAdapter
    {
        private static PersianCalendar pc = new PersianCalendar();

        public DateTime EpochToDateTime(double epochValue) => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epochValue).ToLocalTime();
        public DateTime EpochToDateTime(ReadOnlySpan<char> epochValue) => EpochToDateTime(Convert.ToDouble(epochValue.ToArray()));
        public DateTime EpochToDateTime(string epochValue) => EpochToDateTime(Convert.ToDouble(epochValue));
        public double DateTimeToEpoch(DateTime dateTime) => (int)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        public DateTime PersianToGregorian(int year, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0)
        {
            return DateTime.ParseExact($"{year}-{month.ToString(StringConstants.TwoDigitFormat)}-{day.ToString(StringConstants.TwoDigitFormat)} {hour.ToString(StringConstants.TwoDigitFormat)}:{minute.ToString(StringConstants.TwoDigitFormat)}:{second.ToString(StringConstants.TwoDigitFormat)}", StringConstants.LongDateTimeFormat, new CultureInfo(StringConstants.FaIR));
        }
        public string GregorianToPersian(DateTime dateTime)
        {
            string month = pc.GetMonth(dateTime).ToString();
            string day = pc.GetDayOfMonth(dateTime).ToString();
            string hour = pc.GetHour(dateTime).ToString();
            string minute = pc.GetMinute(dateTime).ToString();
            string second = pc.GetSecond(dateTime).ToString();

            if (!string.IsNullOrEmpty(month) && month.Length == 1) month = StringConstants.Zero + month;
            if (!string.IsNullOrEmpty(day) && day.Length == 1) day = StringConstants.Zero + day;
            if (!string.IsNullOrEmpty(hour) && hour.Length == 1) hour = StringConstants.Zero + hour;
            if (!string.IsNullOrEmpty(minute) && minute.Length == 1) minute = StringConstants.Zero + minute;
            if (!string.IsNullOrEmpty(second) && second.Length == 1) second = StringConstants.Zero + second;

            return $"{pc.GetYear(dateTime)}-{month}-{day} {hour}:{minute}:{second}";
        }

        public string GregorianToSlashPersianDateTime(DateTime dateTime)
        {
            return $"{pc.GetYear(dateTime)}/{pc.GetMonth(dateTime)}/{pc.GetDayOfMonth(dateTime)} {pc.GetHour(dateTime)}:{pc.GetMinute(dateTime)}:{pc.GetSecond(dateTime)}";
        }
        public string GregorianToPersianDateTime(ReadOnlySpan<char> dateTime)
        {
            return GregorianToPersian(Convert.ToDateTime(dateTime.ToArray()));
        }

        public string GregorianToPersianDate(ReadOnlySpan<char> dateTime)
        {
            return GregorianToPersianDate(Convert.ToDateTime(dateTime.ToArray()));
        }

        public string GregorianToPersianDate(DateTime dateTime)
        {
            return $"{pc.GetYear(dateTime)}-{pc.GetMonth(dateTime)}-{pc.GetDayOfMonth(dateTime)}";
        }

        public DateTime PersianToGregorianDateTime(ReadOnlySpan<char> data)
        {
            Span<Range> parts = new Range[6];
            if (data.Length > 0)
            {
                return parts.Length == 3 ? new DateTime(int.Parse(data[parts[0]]), int.Parse(data[parts[1]]), int.Parse(data[parts[2]]), 0, 0, 0, new PersianCalendar())
                    : new DateTime(int.Parse(data[parts[0]]), int.Parse(data[parts[1]]), int.Parse(data[parts[2]]), int.Parse(data[parts[3]]), int.Parse(data[parts[4]]), int.Parse(data[parts[5]]), new PersianCalendar());
            }
            else return new DateTime();
        }
        public DateTime PersianToGregorianDateTime(string data)
        {
            var parts = data.Split(SplitterConstants.DateTimeSplitter, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 3 ? new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), 0, 0, 0, new PersianCalendar())
                : new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]), new PersianCalendar());
        }
    }
}
