using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Enums
{
    public enum ActionStatus
    {
        [Description("exception")]
        Exception,
        [Description("warning")]
        Warning,
        [Description("passed")]
        Pass,
        [Description("failed")]
        Fail,
        [Description("none")]
        None,
    }

    public enum AxiomException
    {
        NoException,
        NullInputException,
        PropertyNotFoundException,
        SourceValueNotFoundException,
        TargetValueNotFoundException,
        OperatorSymbolNotFound,
        ValueTypeConversionError
    }


    public enum AxiomOperator
    {
        [Description("none")]
        Empty,
        [Description("equals")]
        Equals,
        [Description("doesn't equals")]
        NotEquals,
        [Description("is greater than")]
        GreaterThan,
        [Description("is lesser than")]
        LessThan,
        [Description("contains")]
        Contains,
        [Description("doesn not contains")]
        NotContains,
        [Description("starts with")]
        StartsWith,
        [Description("ends with")]
        EndsWith,
        //Between,
    }

    public enum LogicalOperator
    {
        And,
        Or,
    }

    public enum AxiomType
    {
        BinaryAxiom,
        PropertyAxiom,
        PropertyAxiomGeneric,
        ComparisonAxiom,
        ComparisonAxiomGeneric,
        MethodAxiom
    }
}
