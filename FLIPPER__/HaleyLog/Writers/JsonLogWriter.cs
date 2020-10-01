using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using Haley.Abstractions;
using Haley.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Reflection;

namespace Haley.Log.Writers
{
    internal class JSONLogWriter : LogWriterBase
    {
        private const string SUBLOGKEY = "SUBLOG_HOLDER";
        JsonSerializerOptions _options = null;
        public JSONLogWriter(string file_location, string file_name) : base(file_location, file_name , "json") {
            if (_options == null)
            {
                _options = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    IgnoreNullValues = true
                };
                _options.Converters.Add(new JsonStringEnumConverter());
            }
        }


        private object _convert(object source)
        {
            return JsonSerializer.Serialize(source, source.GetType(), _options);
        }

        public override object convert(List<LogBase> memoryData, bool is_sub = false)
        {
            return _convert(memoryData);
        }

        public override object convert(LogBase data, bool is_sub = false)
        {
            return _convert(data);
        }


        public override void write(LogBase data, bool is_sub = false)
        {
            List<LogBase> _towriteList = new List<LogBase>();
            _towriteList.Add(data);
            write(_towriteList, is_sub);
        }

        public override void write(List<LogBase> memoryData, bool is_sub = false)
        {
            List<LogBase> target_list = new List<LogBase>();
            //Now try to get the existing file and see if it has any data.
            if (File.Exists(file_name))
            {
                string _parent_json = File.ReadAllText(file_name);
                target_list = JsonSerializer.Deserialize<List<LogBase>>(_parent_json, _options);
            }
            if (is_sub)
            {
                //If it is sub, then the memorydata goes into the last node.
                if (target_list.Count == 0)
                {
                    //If target doens't have any data, then add a new one
                    target_list.Add(new LogBase() {Name = SUBLOGKEY,TimeStamp = DateTime.UtcNow });
                }
                target_list.Last().Children.AddRange(memoryData);
            }
            else
            {
                target_list.AddRange(memoryData);
            }
            string _towrite = (string)convert(target_list, is_sub);
            File.WriteAllText(file_name, _towrite);
        }
    }
}
