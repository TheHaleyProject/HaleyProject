using System;
using System.Collections.Generic;
using System.Text;
using Haley.Abstractions;
using Haley.Enums;

namespace Haley.Models
{
    public class KeyBase : IEquatable<KeyBase>
    {
        public string id { get;  }
        public string priority { get;}
        public Type contract_type { get;  }

        public static string generate(Type contract_type,string priority_key)
        {
                //Key = $contractType##$priority_key
                string _key = "";
                if (contract_type != null)
                {
                    _key = $@"{contract_type.ToString()}";
                }
                if (!string.IsNullOrEmpty(priority_key) || !string.IsNullOrWhiteSpace(priority_key))
                {
                    if (!string.IsNullOrEmpty(_key)) _key += "###";
                    _key += priority_key.ToLower();
                }
                return _key;
        }
        public override bool Equals(object obj)
        {
            var item = obj as KeyBase;
            if (item == null)
            {
                return false;
            }
            return this.id.Equals(item.id);
         }
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }
        public override string ToString()
        {
            return id;
        }
        public bool Equals(KeyBase other)
        {
            if (other == null)
            {
                return false;
            }
            return this.id == other.id;
        }

        [HaleyIgnore]
        public KeyBase(Type _contract_type, string _priority_key) 
        {
            var _key = KeyBase.generate(_contract_type, _priority_key);
            id = _key;
            priority = _priority_key;
            contract_type = _contract_type;
        }
    }
}
