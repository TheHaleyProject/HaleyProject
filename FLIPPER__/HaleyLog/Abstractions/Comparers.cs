using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Haley.Abstractions
{
    public class LogEqualityComparer : IEqualityComparer<ILog>
    {
        public bool Equals(ILog x, ILog y)
        {
            return (x.Id == y.Id);
        }

        public int GetHashCode(ILog obj)
        {
            return obj.GetHashCode();
        }
    }
}
