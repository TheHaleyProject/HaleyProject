using Haley.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Haley.Enums;
using Haley.Log.Writers;
using System;
using System.IO;
using System.Linq;

namespace Haley.Abstractions
{
    public interface ILogger
    {
            string debug(string message, string property_name = null, bool in_memory = false, bool is_sub = false);
            string debug(Exception exception, string comments = null, string property_name = null, bool in_memory = false, bool is_sub = false);
            string debug(string key, string value, string comments = null, string property_name = null, bool in_memory = false, bool is_sub = false);
            string log(string message, MessageType msg_type = MessageType.information, string property_name = null, bool in_memory = false, bool is_sub = false);
            string log(Exception exception, string comments = null, string property_name = null, bool in_memory = false, bool is_sub = false);
            string log(string key, string value, string comments = null, string property_name = null, bool in_memory = false, bool is_sub = false);
            void dumpMemory();
            string getDirectory();
    }
}
