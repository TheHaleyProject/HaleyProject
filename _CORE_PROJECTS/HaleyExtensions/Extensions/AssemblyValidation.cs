using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
using System.Diagnostics;

namespace Haley.Utils
{
    public static class AssemblyValidation
    {
        public static bool IsDebugBuild(this Assembly assembly)
        {
            if (assembly == null)
            {
                return false;
            }

            return assembly.GetCustomAttribute<DebuggableAttribute>()?.IsJITTrackingEnabled ?? false;
        }
    }
}