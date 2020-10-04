using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.Models
{
    public class Rule
    {
        public string id { get; }
        public string name { get; set; }
        public string description { get; set; }
        public ActionStatus status { get; set; }
        public RuleBlock block { get; set; }

        #region Override Methods
        public override string ToString()
        {
            return $@"{name} : {status.ToString()}";
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var _obj = (Rule)obj;

            return this.id == _obj.id;
        }
        #endregion

        public Rule(string _name) { id = Guid.NewGuid().ToString(); name = _name; block = new RuleBlock(); status = ActionStatus.None; }
    }
}
