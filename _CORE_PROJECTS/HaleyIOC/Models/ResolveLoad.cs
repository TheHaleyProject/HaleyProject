using System;
using System.Collections.Generic;
using System.Text;
using Haley.Enums;
using Haley.Abstractions;

namespace Haley.Models
{
    public sealed class ResolveLoad :LoadBase
    {
        public string contract_name { get; set; }
        public Type contract_parent { get; set; }
        public ResolveMode mode { get; set; }

        [HaleyIgnore]
        public ResolveLoad(ResolveMode _mode,string _priority_key,string _contract_name, Type _contract_type, Type _contract_parent ,Type _concrete_type, TransientCreationLevel _transient_level = TransientCreationLevel.None):base(_priority_key,_contract_type,_concrete_type,_transient_level)
        {
            //Nullables
            contract_name = _contract_name;
            contract_parent = _contract_parent;
            mode = _mode;
        }
        [HaleyIgnore]
        public ResolveLoad():base(null,null,null)
        {
            mode = ResolveMode.AsRegistered;
        }
    }
}
