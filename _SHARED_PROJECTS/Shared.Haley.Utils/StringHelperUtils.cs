using System;
using System.Reflection;
using System.ComponentModel;

namespace Haley.Utils
{
    public static class StringHelpers
    {
        public static string getEnumAsKey(Enum @enum)
        {
            try
            {
                string enum_type_name = @enum.GetType().Name;
                string enum_value_name = @enum.ToString();
                string enum_key = enum_type_name + "." + enum_value_name; //Concatenated value for storing as key
                return enum_key;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string getEnumDesctiption(Enum @enum)
        {
            FieldInfo fi = @enum.GetType().GetField(@enum.ToString());
            var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length == 0 ? @enum.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }
        public static string getString(object value)
        {
            if (value == null) return null;
            Type InputType = value.GetType();
            return (Convert.ChangeType(value, value.GetType()))?.ToString() ?? null;
        }

        public static object getObject(object value, Type targetType)
        {
            if (targetType == typeof(string)) return (string)value;
            if (targetType == typeof(int)) return int.Parse((string)value);
            if (targetType == typeof(double)) return double.Parse((string)value);
            if (targetType == typeof(bool)) return bool.Parse((string)value);
            if (targetType.BaseType == typeof(Enum)) return value;
            return value;
        }
    }
}
