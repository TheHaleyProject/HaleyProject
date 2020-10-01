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
using System.Data;

namespace Haley.Log.Writers
{
    internal class SimpleTextWriter :LogWriterBase
    {
        public SimpleTextWriter(string file_location, string file_name) : base(file_location, file_name , "txt") { }

        public override void write(LogBase data, bool is_sub = false)
        {
            string _towrite = (string)convert(data,is_sub);
            using (StreamWriter swriter = File.AppendText(file_name))
            {
               swriter.WriteLine(_towrite);
            }
        }
        public override void write(List<LogBase> memoryData, bool is_sub = false)
        {
            string _towrite = (string)convert(memoryData,is_sub);
            using (StreamWriter swriter = File.AppendText(file_name))
            {
               swriter.WriteLine(_towrite);
            }
        }
        public override object convert(List<LogBase> memoryData, bool is_sub = false)
        {
            StringBuilder mainbuilder = new StringBuilder();
            foreach (var item in memoryData)
            {
                //Get the primary values
                mainbuilder.AppendLine((string) convert(item, is_sub)); //convert each sinlge entry
                if (item.Children.Count > 1)
                {
                    mainbuilder.AppendLine((string)convert(item.Children,true));
                }
            }
            return mainbuilder.ToString();
        }
        public override object convert(LogBase data, bool is_sub = false)
        {
            StringBuilder sbuilder = new StringBuilder();
            sbuilder.Append(string.Format("{0,-20}", data.TimeStamp.ToString(timeformat)));
            sbuilder.Append(" | ");
            sbuilder.Append(string.Format("{0,-6}", data.MessageType.ToString().Substring(0,4).ToUpper()));
            sbuilder.Append(" | ");
            //If Property name is not null, provide it.
            if (!string.IsNullOrEmpty(data.Name))
            {
                sbuilder.Append("Prop Name - " + data.Name);
                sbuilder.Append(" | ");
            }
            //Add if KVP
            if (data.GetType() == typeof(DictionaryLog))
            {
                sbuilder.Append("Key : " + ((DictionaryLog)data).Key);
                sbuilder.Append(" | ");
                sbuilder.Append("Value : " + ((DictionaryLog)data).Value);
                sbuilder.Append(" | ");
            }
            //Add message
            if (!string.IsNullOrEmpty(data.Message))
            {
                sbuilder.Append(data.Message);
                sbuilder.Append(" | ");
            }
            //Add if dictionary
            if (data.GetType() == typeof(ExceptionLog))
            {
                sbuilder.Append("Exception Message : " + ((ExceptionLog)data).ExceptionMessage);
                sbuilder.Append(" | ");
                sbuilder.Append("Trace : " + ((ExceptionLog)data).Trace);
                sbuilder.Append(" | ");
            }

            if (is_sub) return ($@"***SUBLOG*** | " + sbuilder.ToString()); //if sub add sub log 
            return sbuilder.ToString();
        }
    }
}
