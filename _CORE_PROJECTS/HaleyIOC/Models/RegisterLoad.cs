using System;
using System.Collections.Generic;
using System.Text;
using Haley.Enums;
using Haley.Abstractions;

namespace Haley.Models
{
    public sealed class RegisterLoad :LoadBase
    {
        public object concrete_instance { get; set; }
        public RegisterMode mode { get; set; }
        public ResolveLoad convert(string contract_name, Type contract_parent, ResolveMode mode)
        {
            ResolveLoad _load = new ResolveLoad(mode, this.priority_key, contract_name, this.contract_type, contract_parent, this.concrete_type, this.transient_level);
            return _load;
        }

        [HaleyIgnore]
        public RegisterLoad(RegisterMode _mode,string _priority_key, Type _contract_type, Type _concrete_type,object _concrete_instance, TransientCreationLevel _transient_level = TransientCreationLevel.None):base(_priority_key,_contract_type,_concrete_type, _transient_level)
        {
            concrete_instance = _concrete_instance;
            mode = _mode;
        }
        [HaleyIgnore]
        public RegisterLoad():base(null,null,null)
        {
            mode = RegisterMode.Singleton;
        }
    }
}
