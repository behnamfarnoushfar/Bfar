﻿using System.Collections.Generic;
using System.Data;

namespace Bfar.Extensions
{
    public static class ObjectExtensions
    {
        public static DataRow ToDataRow<T>(this T value)
        {
            List<T> list = new List<T>();
            list.Add(value);
            return list.ToDataTable().Rows[0];
        }

        public static bool IsNull<T>(this T value)
        {
            return value == null;
        }

        public static bool IsNotNull<T>(this T value)
        {
            return value != null;
        }
    }
}
