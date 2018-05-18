using System.Collections.Generic;
using System.Linq;

namespace Bfar.Extensions
{
    public static class StringExtension
    {
        public static Dictionary<string, string> ToDictionary(this string data)
        {
            return data.Split('&')
              .Select(value => value.Split('='))
              .ToDictionary(pair => pair[0], pair => pair[1]);
        }
        public static Dictionary<string, string> ToDictionary(this string data, char paramSepartor, char valueSepartor)
        {
            return data.Split(paramSepartor)
              .Select(value => value.Split(valueSepartor))
              .ToDictionary(pair => pair[0], pair => pair[1]);
        }
    }
}
