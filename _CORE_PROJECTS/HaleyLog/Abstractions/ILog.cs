using Haley.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Haley.Enums;

namespace Haley.Abstractions
{
   public interface ILog
    {
        string Name { get; set; }
        DateTime TimeStamp { get; set; }
        string Message { get; set; }
        string Id { get; set; }
        MessageType MessageType { get; set; }
    }
}
