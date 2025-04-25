namespace Bfar.Extensions.Persian
{
    public static class Int64Extensions
    {
        public static bool IsValidNationalCode(this long data)
        {
            return data.ToString().IsValidNationalCode();
        }
    }
}
