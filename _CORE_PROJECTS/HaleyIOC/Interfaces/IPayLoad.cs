using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using Haley.Enums;

namespace Haley.Abstractions
{
   public interface IPayLoad
    {
        string priority_key { get; set; }
        Type contract_type { get; set; }
        Type concrete_type { get; set; }
        TransientCreationLevel transient_level { get; set; }
    }
}
