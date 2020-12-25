﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Reflection;

namespace Haley.Utils
{
    public static class ResourceUtils
    {
        public static bool downloadEmbeddedResource(string resource_name, Assembly assembly_name, string save_dir_path, string save_file_name = null)
        {
            try
            {
                if (save_file_name == null) save_file_name = resource_name; //use same resource name as target name
                string full_file_path = Path.Combine(save_dir_path, save_file_name);
                return downloadEmbeddedResource(resource_name, assembly_name, full_file_path);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool downloadEmbeddedResource(string resource_name, Assembly assembly_name, string save_file_path)
        {
            try
            {
                var _stream = assembly_name.GetManifestResourceStream(resource_name); //Get the resource from the assembly
                byte[] _stream_byte = new byte[_stream.Length]; //initiate a byte array

                using (var memstream = new MemoryStream())
                {
                    _stream.CopyTo(memstream);
                    _stream_byte = memstream.ToArray();
                }

                File.WriteAllBytes(save_file_path, _stream_byte);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static string getEmbeddedResource(string full_resource_name, Assembly assembly_name)
        {
            try
            {
                string _result = string.Empty;
                var _stream = assembly_name.GetManifestResourceStream(full_resource_name); //Get the resource from the assembly
                if (_stream == null) return null;
                using (StreamReader sreader = new StreamReader(_stream))
                {
                    _result = sreader.ReadToEnd();
                }

                return _result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
