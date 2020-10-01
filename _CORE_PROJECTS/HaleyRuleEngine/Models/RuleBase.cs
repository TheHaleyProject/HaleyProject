using System;
using System.Collections.Generic;
using System.Text;
using Haley.Enums;

namespace Haley.Models
{
    public abstract class RuleBase
    {
        public Guid ID { get; set; }
        public string property_name { get; set; }
        public ExpressionType Operator { get; set; }
        public DLEngineExpressionTypes DLEngine_Operator { get; set; }
        public bool IIs_DLEngine_Rule { get; set; }
        public object Value { get; set; }
        public object Value2 { get; set; }
        public string Details { get; set; }
        public override string ToString()
        {
            StringBuilder strbldr = new StringBuilder();
            strbldr.Append(this.Key + " : ");
            bool isTwoValue = false;
            switch (this.IIs_DLEngine_Rule)
            {
                case true:
                    strbldr.Append(this.DLEngine_Operator.ToString());
                    if (this.DLEngine_Operator == DLEngineExpressionTypes.Between) isTwoValue = true;
                    break;
                case false:
                    strbldr.Append(this.Operator.ToString()); //Expression type doesn't have between, so no twovalue
                    break;
            }
            strbldr.Append(" " + this.Value.ToString());
            if (isTwoValue) strbldr.Append(" and " + this.Value2.ToString());
            return strbldr.ToString();
        }
        public Rule() { if (ID == Guid.Empty) ID = Guid.NewGuid(); IIs_DLEngine_Rule = false; } //Whenever a rule is initiated, assign a guid to it. So that we can investigate it later.
    }
}
