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

    public enum GenerateNewInstance
    {
        /// <summary>
        /// Instance will not be created. Singleton object will be returned to the user.
        /// </summary>
        None,
        /// <summary>
        /// Only the current type will get new instance.
        /// </summary>
        TargetOnly,
        /// <summary>
        /// All the sub type dependencies will generate new instance.
        /// </summary>
        AllDependencies
    }

    public enum InjectionTarget
    {
        All,
        Constructor,
        Property
    }
}
