using Haley.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Haley.Enums;

namespace Haley.Abstractions
{
    public interface ILogWriter
    {
        object convert(List<LogBase> memoryData, bool is_sub = false); //To Convert to relevant format
        object convert(LogBase data, bool is_sub = false); //To Convert to relevant format
        void write(LogBase data,bool is_sub=false);
        void write(List<LogBase> memoryData, bool is_sub = false);
    }
}
