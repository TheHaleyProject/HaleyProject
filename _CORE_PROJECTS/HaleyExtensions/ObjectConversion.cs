using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Linq;
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
        public static object changeType<T>(this object value, ta)
        {
            if (!value.IsList()) return (T)value;
            List<object> _value_list = value as List<object>;
            Type _valuetype = _value_list.First().GetType();
            Type targetType = typeof(T);
            List<object> _result_list = new List<object>();

            foreach (var item in _value_list)
            {
                var _actual_type = Convert.ChangeType(item, item.GetType());
                _result_list.Add(item);
            }
            if (targetType.IsList())
            {
                return _result_list;
            }
            else if (targetType.IsCollection())
            {
                ICollection<object> _result_collection = _result_list;
                return Convert.ChangeType(_result_collection, targetType);
            }
            else if (targetType.IsEnumerable())
            {
                IEnumerable<object> _result_collection = _result_list;
                return Convert.ChangeType(_result_collection, targetType);
            }
            else if (targetType.IsArray)
            {
                return Convert.ChangeType(_result_list.ToArray(), targetType);
            }
            return null;
        }

        public static object ConvertList(List<object> items, Type type, bool performConversion = false)
        {
            var containedType = type.GenericTypeArguments.First();
            var enumerableType = typeof(System.Linq.Enumerable);
            var castMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.Cast)).MakeGenericMethod(containedType);
            var toListMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.ToList)).MakeGenericMethod(containedType);

            IEnumerable<object> itemsToCast;

            if (performConversion)
            {
                itemsToCast = items.Select(item => Convert.ChangeType(item, containedType));
            }
            else
            {
                itemsToCast = items;
            }

            var castedItems = castMethod.Invoke(null, new[] { itemsToCast });

            return toListMethod.Invoke(null, new[] { castedItems });
        }
    }
}