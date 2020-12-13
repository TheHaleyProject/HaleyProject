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
    public static class IDUtils
    {
        public static string getMotherBoardID()
        {
            try
            {
                var mng_obj_searcher = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
                var collection = mng_obj_searcher.Get();
                string id = null;
                try
                {
                    foreach (ManagementObject mgt_obj in collection)
                    {
                        id = (string)mgt_obj["SerialNumber"];
                    }
                }
                catch (Exception)
                {
                    return id;
                }
                return id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static string getProcessorID()
        {
            try
            {
                var mng_obj_searcher = new ManagementObjectSearcher("Select * From Win32_processor");
                var collection = mng_obj_searcher.Get();
                string id = null;
                try
                {
                    foreach (ManagementObject mgt_obj in collection)
                    {
                        id = (string)mgt_obj["ProcessorId"];
                    }
                }
                catch (Exception)
                {
                    return id;
                }
                return id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string getID()
        {
            try
            {
                return (getMotherBoardID() + "###" +getProcessorID());
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
