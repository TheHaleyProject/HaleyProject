using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Haley.Utils
{
  public static class Extensions
        {
            public static bool isList(this object obj)
            {
                try
                {
                    if (obj == null) return false;
                    if (obj is IList && obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))) return true;
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public static bool isDictionary(this object obj)
            {
                try
                {
                    if (obj == null) return false;
                    if (obj is IDictionary && obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>))) return true;
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
}