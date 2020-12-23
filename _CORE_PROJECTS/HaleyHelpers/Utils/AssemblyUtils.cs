using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Reflection;

namespace Haley.Utils
{
    public static class AssemblyUtils
    {
        public static void forceLoadDependencies(AssemblyName[] reference_name_array)
        {
            try
            {
                //AssemblyName[] reference_name_array = Assembly.GetExecutingAssembly().GetReferencedAssemblies(); //Force load all referenced assemblies into memory.
                foreach (var assname in reference_name_array)
                {
                    List<Assembly> loaded_assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
                    if (!(loaded_assemblies.Any(p => p.GetName().Name == assname.Name)))
                    {
                        Assembly.Load(assname);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
