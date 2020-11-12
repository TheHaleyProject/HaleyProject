using System;
using System.Linq;

namespace Haley.Enums
{
    public enum ResizeAffectMode
    {
        pixelSize,
        pixelSize_dpi,
        pixelSize_imageSize,
    }

    public enum GenerateNewInstanceFor
    {
        None,
        TargetObjectOnly,
        TargetObjectWithParameters,
        CascadeAll
    }

    public enum InjectionTarget
    {
        All,
        Constructor,
        Property
    }
}
