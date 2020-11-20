using System;
using System.Collections.Generic;
using System.Text;
using Haley.Abstractions;
using Haley.Enums;

namespace Haley.Models
{
    public class LoadBase : IPayLoad
    {
        public string priority_key { get; set; }
        public Type contract_type { get; set; }
        public Type concrete_type { get; set; }
        public TransientCreationLevel transient_level { get; set; }

        [HaleyIgnore]
        public LoadBase(string _priority_key, Type _contract_type, Type _concrete_type, TransientCreationLevel _transient_level = TransientCreationLevel.None) 
        {
            priority_key = _priority_key;
            contract_type = _contract_type;
            concrete_type = _concrete_type ?? _contract_type;
            transient_level = _transient_level;
        }
    }
}
