using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;

namespace Bfar.Extensions.Core
{
    public static class ListExtensions
    {
        public delegate string TranslateColumnNameCallBack(string Name);
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            if (data == null)
                return null;
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        public static DataTable ToDataTable<T>(this IList<T> data, bool UsePersianCalendar = false, TranslateColumnNameCallBack callback = null)
        {
            if (data == null || data.Count == 0)
                return null;
            Regex dtx = new Regex(@"^\d\d\d\d-(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01]) (00|[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]):([0-9]|[0-5][0-9])$");
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                //Type type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(callback != null ? callback(prop.Name) : prop.Name/*, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType*/);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (props[i].GetValue(item) != null && ((Nullable.GetUnderlyingType(props[i].PropertyType) ?? props[i].PropertyType) == typeof(DateTime)
                        || (dtx.IsMatch(props[i].GetValue(item).ToString())))
                        && UsePersianCalendar)
                    {

                        try
                        {

                            values[i] = Convert.ToDateTime(props[i].GetValue(item).ToString(), System.Globalization.CultureInfo.GetCultureInfo("en").DateTimeFormat)/*.ToFullPersianString()*/;
                        }
                        catch (Exception ex)
                        {

                            values[i] = props[i].GetValue(item);
                        }
                    }
                    else
                    {
                        values[i] = props[i].GetValue(item);

                    }
                }
                table.Rows.Add(values);
            }
            return table;
        }
        public static DataTable ToDataTable<T>(this IList<T> data, string Name)
        {
            if (data == null)
                return null;
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable(Name);
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
}
