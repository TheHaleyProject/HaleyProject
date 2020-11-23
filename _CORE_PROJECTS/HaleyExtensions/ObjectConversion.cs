using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.ComponentModel;

namespace Haley.Utils
{
    public static class ObjectConversion
    {
        public static string asString(this object value)
        {
            if (value == null) return null;
            Type InputType = value.GetType();
            return (Convert.ChangeType(value, value.GetType()))?.ToString() ?? null;
        }
        public static object changeType(this object value, Type targetType)
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