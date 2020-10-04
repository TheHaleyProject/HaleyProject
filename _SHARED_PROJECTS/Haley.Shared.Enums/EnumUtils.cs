using System;
using System.Reflection;
using System.ComponentModel;

namespace Haley.Utils
{
    internal static class StringHelpers
    {
        public static string getEnumDesctiption(Enum @enum)
        {
            FieldInfo fi = @enum.GetType().GetField(@enum.ToString());
            var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length == 0 ? @enum.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }
        public static string getString(object value)
        {
            Type InputType = value.GetType();
            //Convert Input to String.
            if (InputType == typeof(string)) return (string)value;
            if (InputType == typeof(int)) return ((int)value).ToString();
            if (InputType == typeof(double)) return ((double)value).ToString();
            if (InputType == typeof(bool)) return ((bool)value).ToString();
            if (InputType.BaseType == typeof(Enum)) return ((Enum)value).ToString();
            return null;
        }

        public static object getObject(object value, Type targetType)
        {
            if (targetType == typeof(string)) return (string)value;
            if (targetType == typeof(int)) return int.Parse((string)value);
            if (targetType == typeof(double)) return double.Parse((string)value);
            if (targetType == typeof(bool)) return bool.Parse((string)value);
            if (targetType.BaseType == typeof(Enum)) return value;
            return null;
        }
    }
}
