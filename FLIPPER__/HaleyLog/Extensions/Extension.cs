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
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Haley.Log.Extensions
{
    public static class Extension
    {
        public static XElement ToXml(this object source)
        {
            Type _type = source.GetType();

            #region Abandoned - To Consider interfaces
            ////If the source has any Interface properties, we just get them as extratypes.
            //Type[] extraTypes = _type.GetProperties()
            //    .Where(p => p.PropertyType.IsInterface)
            //    .Select(p => p.GetValue(source, null).GetType())
            //    .ToArray();

            //DataContractSerializer serializer = new DataContractSerializer(_type, extraTypes);
            //serializer.WriteObject(xw, source);
            #endregion

            XmlSerializer serializer = new XmlSerializer(_type);

            //TO IGNORE UNWANTED NAMESPACES
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            serializer.Serialize(xw, source, ns);
            return XElement.Parse(sw.ToString());
        }
    }
}
