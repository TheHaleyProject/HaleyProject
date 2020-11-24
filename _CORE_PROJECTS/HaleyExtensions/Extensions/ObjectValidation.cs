using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.ComponentModel;

namespace Haley.Utils
{
    public static class ObjectValidation
    {
        public static bool IsList(this object obj)
        {
            try
            {
                if (obj == null) return false;
                if (obj is Type)
                {
                    Type _type = (Type)obj;
                    if (_type.IsGenericType && _type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))) return true;
                }
                else
                {
                    if (obj is IList && obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))) return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool IsDictionary(this object obj)
        {
            try
            {
                if (obj == null) return false;
                if (obj is Type)
                {
                    Type _type = (Type)obj;
                    if (_type.IsGenericType && _type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>))) return true;
                }
                else
                {
                    if (obj is IDictionary && obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>))) return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool IsEnumerable(this object obj)
        {
            try
            {
                if (obj == null) return false;
                if (obj is Type)
                {
                    Type _type = (Type)obj;
                    if(_type.IsGenericType && _type.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>))) return true;
                }
                else
                {
                    if (obj is IEnumerable && obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>))) return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool IsCollection(this object obj)
        {
            try
            {
                if (obj == null) return false;
                if (obj is Type)
                {
                    Type _type = (Type)obj;
                    if (_type.IsGenericType && _type.GetGenericTypeDefinition().IsAssignableFrom(typeof(ICollection<>))) return true;
                }
                else
                {
                    if (obj is ICollection && obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(ICollection<>))) return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}