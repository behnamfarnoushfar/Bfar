namespace Bfar.XCutting.Foundation.Extensions
{
    public static class CreditExtensions
    {
        public static string BitTrafficFormatter(this long data)
        {
            bool negativeNumber = data < 0;

            if (negativeNumber) data = -1 * data;

            if (data < 8)
                return string.Format("{0}{1}", negativeNumber ? "-" : string.Empty, data.ToString("N2") + " b");
            if (data < 1024)
                return string.Format("{0}{1}", negativeNumber ? "-" : string.Empty, (data / 8.0).ToString("N2") + " B");
            if (data < 1024 * 1024)
                return string.Format("{0}{1}", negativeNumber ? "-" : string.Empty, (data / 8.0 / 1024.0).ToString("N2") + " KB");
            if (data < 1024 * 1024 * 1024)
                return string.Format("{0}{1}", negativeNumber ? "-" : string.Empty, (data / 8.0 / 1024.0 / 1024.0).ToString("N2")) + " MB";
            if (data < 1024.0 * 1024.0 * 1024.0 * 1024.0)
                return string.Format("{0}{1}", negativeNumber ? "-" : string.Empty, (data / 8.0 / 1024.0 / 1024.0 / 1024.0).ToString("N2")) + " GB";
            //if (data < 1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0)
            return string.Format("{0}{1}", negativeNumber ? "-" : string.Empty, (data / 8.0 / 1024.0 / 1024.0 / 1024.0 / 1024.0).ToString("N2")) + " TB";
        }
    }
}
