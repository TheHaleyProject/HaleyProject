using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using Haley.Log.Interfaces;
using Haley.Log.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Runtime;

namespace Haley.Log.Writers
{
    internal abstract class LogWriterBase : ILogWriter
    {
        public string file_location { get; set; }
        public string file_name { get; set; }
        public string timeformat = "yyyy-MM-dd HH:mm:ss";
        public LogWriterBase(string _file_location, string _file_name, string _extension)
        {
            file_location = _file_location;
            file_name = Path.Combine(_file_location, _file_name+ "." + _extension);
        }

        public abstract object convert(List<LogBase> memoryData, bool is_sub = false);

        public abstract object convert(LogBase data, bool is_sub = false);

        public abstract void write(LogBase data, bool is_sub = false);

        public abstract void write(List<LogBase> memoryData, bool is_sub = false);
    }
}
