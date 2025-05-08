namespace Bfar.XCutting.Abstractions.Decorators
{
    public interface IDateTimeAdapter
    {
        double DateTimeToEpoch(DateTime dateTime);
        DateTime PersianToGregorian(int year, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0);
        DateTime EpochToDateTime(double epochValue);
        DateTime EpochToDateTime(ReadOnlySpan<char> epochValue);
        string GregorianToPersianDateTime(ReadOnlySpan<char> dateTime);
        string GregorianToPersianDate(ReadOnlySpan<char> dateTime);
        string GregorianToSlashPersianDateTime(DateTime dateTime);
        string GregorianToPersianDate(DateTime dateTime);
        DateTime PersianToGregorianDateTime(ReadOnlySpan<char> data);
        DateTime PersianToGregorianDateTime(string data);
        string GregorianToPersian(DateTime dateTime);
    }
}
