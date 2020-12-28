using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Linq;
using System.ComponentModel;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using ProtoBuf.Serializers;
using ProtoBuf;

namespace Haley.Utils
{
    public static class ObjectSerialization
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
        public static string binarySerialize(this object input)
        {
            string result = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, input);
                    result = Convert.ToBase64String(ms.ToArray());
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static object binaryDeserialize(this string input)
        {
            object result = null;
            try
            {
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(input)))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    result = bf.Deserialize(ms);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static T binaryDeserialize<T>(this string input)
        {
            return  (T)input.binaryDeserialize();
        }
        public static string protoSerialize(this object input)
        {
            string result = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, input);
                    result = Convert.ToBase64String(ms.ToArray());
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static T protoDeserialize<T>(this string input)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(input)))
                {
                   return Serializer.Deserialize<T>(ms);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
 }
