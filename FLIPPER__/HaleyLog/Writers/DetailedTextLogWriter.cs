using Haley.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Haley.Log.Writers
{
    internal class DetailedTextLogWriter : LogWriterBase
    {
        public DetailedTextLogWriter(string file_location, string file_name) : base(file_location, file_name, "txt") { }

        public override void write(LogBase data, bool is_sub = false)
        {
            string _towrite = (string)convert(data, is_sub);
            using (StreamWriter swriter = File.AppendText(file_name))
            {
                swriter.WriteLine(_towrite);
            }
        }
        public override void write(List<LogBase> memoryData, bool is_sub = false)
        {
            string _towrite = (string)convert(memoryData, is_sub);
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
                mainbuilder.AppendLine((string)convert(item, is_sub)); //convert each sinlge entry
                if (item.Children.Count > 1)
                {
                    mainbuilder.AppendLine((string)convert(item.Children, true));
                }
            }
            return mainbuilder.ToString();
        }
        public override object convert(LogBase data, bool is_sub = false)
        {
            StringBuilder sbuilder = new StringBuilder();
            if (is_sub)
            {
                sbuilder.AppendLine("#####*****---- BEGIN SUB LOG ----*****#####");
            }
            else
            {
                sbuilder.AppendLine("---- BEGIN LOG ----");
            }
            //Get timestamp
            sbuilder.AppendLine(nameof(data.TimeStamp) + " : " + data.TimeStamp.ToString(timeformat));
            //Get PropertyName
            if (!string.IsNullOrEmpty(data.Name)) sbuilder.AppendLine(nameof(data.Name) + " : " + data.Name);
            //Get the main message if present
            if (!string.IsNullOrEmpty(data.Message)) sbuilder.AppendLine(nameof(data.Message) + " : " + data.Message);
            //Get the Info Type
            sbuilder.AppendLine(nameof(data.MessageType) + " : " + data.MessageType.ToString());

            //Get Further Data if it is exception type
            if (data.GetType() == typeof(ExceptionLog))
            {
                ExceptionLog _excplog = (ExceptionLog)data;
                if (!string.IsNullOrEmpty(_excplog.ExceptionMessage)) sbuilder.AppendLine(nameof(_excplog.ExceptionMessage) + " : " + _excplog.ExceptionMessage);
                if (!string.IsNullOrEmpty(_excplog.Trace)) sbuilder.AppendLine(nameof(_excplog.Trace) + " : " + _excplog.Trace);
            }

            //Get data if it is property type
            if (data.GetType() == typeof(DictionaryLog))
            {
                DictionaryLog _dicLog = (DictionaryLog)data;
                if (!string.IsNullOrEmpty(_dicLog.Key)) sbuilder.AppendLine(nameof(_dicLog.Key) + " : " + _dicLog.Key);
                if (!string.IsNullOrEmpty(_dicLog.Value)) sbuilder.AppendLine(nameof(_dicLog.Value) + " : " + _dicLog.Value);
            }

            if (is_sub)
            {
                sbuilder.AppendLine("#####*****---- END SUB LOG ----*****#####");
            }
            else
            {
                sbuilder.AppendLine("---- END LOG ----");
            }

            return sbuilder.ToString();
        }
    }
}
