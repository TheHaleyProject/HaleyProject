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

        public static string getSignedKey(this Assembly assembly)
        {
            try
            {
                if (assembly == null) return null;
                var _asm_name = assembly.GetName();
                var _key = _asm_name.GetPublicKey();
                return Convert.ToBase64String(_key);
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}