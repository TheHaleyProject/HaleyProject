using Haley.Abstractions;
using Haley.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Haley.Models
{
    [XmlRoot("BaseLog")]
    public class LogBase : ILog
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("Time")]
        public DateTime TimeStamp { get; set; }
        [XmlAttribute("MsgType")]
        public MessageType MessageType { get; set; }
        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }

        [XmlArrayItem(Type = typeof(DictionaryLog), ElementName = "Property"),
            XmlArrayItem(Type = typeof(ExceptionLog), ElementName = "Exception"),
            XmlArrayItem(Type = typeof(LogBase), ElementName = "BaseLog")]
        public virtual List<LogBase> Children { get; set; }

        #region Serialization Ignoring Items
        [JsonIgnore]
        [XmlIgnore]
        public bool ChildrenSpecified { get { return Children.Count > 0; } } // {propertyName}Specified will return a bool to ensure whether the particular property is specified in export or not.
        [JsonIgnore]
        [XmlIgnore]
        public bool NameSpecified { get { return !string.IsNullOrEmpty(Name); } }
        [JsonIgnore]
        [XmlIgnore]
        public bool MessageSpecified { get { return !string.IsNullOrEmpty(Message); } }
        #endregion
        public LogBase() { Id = Guid.NewGuid().ToString(); Children = new List<LogBase>(); }
    }

    [XmlRoot("Property")]
    public class DictionaryLog : LogBase
    {
        [XmlAttribute(AttributeName = "Key")]
        public string Key { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot("Exception")]
    public class ExceptionLog : LogBase
    {
        [XmlElement(ElementName = "Source")]
        public string Source { get; set; }
        [XmlElement(ElementName = "Trace")]
        public string Trace { get; set; }
        [XmlElement(ElementName = "Exception")]
        public string ExceptionMessage { get; set; }
    }
}
