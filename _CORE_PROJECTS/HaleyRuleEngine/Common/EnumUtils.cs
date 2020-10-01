using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Enums
{
    public enum RuleResponse
    {
        Unknown,
        Success,
        Failed,
        PropertyNotFoundException ,
        SourceValueNotFoundException,
        Not ,
        OperatorSymbolNotFound,
        TargetValue1NotFound,
        TargetValue2NotFound,
        ValueTypeConversionError,
        ValidationLogicMissingForOperator 
    }

    public enum RuleOperator
    {
        Unknown,
        Equals,
        DoesntEquals,
        GreaterThan,
        LessThan,
        Contains,
        StartsWith,
        DoesntContains ,
        Between,
        And,
        Or,
        AndAlso,
        OrElse
    }
}
