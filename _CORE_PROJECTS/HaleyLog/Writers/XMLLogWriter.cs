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
using Haley.Utils;
using System.Xml;

namespace Haley.Log.Writers
{
    internal class XMLLogWriter : LogWriterBase
    {
        public XMLLogWriter(string file_location, string file_name) : base(file_location, file_name, "xml") { }

        #region Private Methods
        private const string ROOTNAME = "HLOG";
        private const string DEFAULTELEMENT = "SUBLOG_HOLDER";
        private XDocument _getXDocument()
        {
            try
            {
                //Try to load the file from filepath. If it is empty, create a new xdocument.
                XDocument xdoc;
                try
                {
                    xdoc = XDocument.Load(file_name);
                }
                catch (Exception)
                {
                    xdoc = new XDocument(new XElement(ROOTNAME));
                }
                return xdoc;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private XElement _getRoot(XDocument xdoc)
        {
            try
            {
                //Try to get the root and check if it is valid. If it is not valid, create a new xdocument and set the root.
                XElement xroot = xdoc.Root;
                if (xroot.Name != ROOTNAME || xroot == null)
                {
                    xdoc = new XDocument(new XElement(ROOTNAME));
                    xroot = xdoc.Root;
                }
                return xroot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private object _convert(object source)
        {
            return source.ToXml();
        }
        #endregion

        #region Overridden Methods

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
            //If sub, read the xml and get the last node and add everything as sub.
            try
            {
                //Get Xdocument and the root element.
                XDocument xdoc = _getXDocument();
                XElement xroot = _getRoot(xdoc);
                XElement input_node = (XElement)convert(data, is_sub);

                if (is_sub)
                {
                    //if sub, find the last node and add to it.
                    if (xroot.Elements().Count() == 0) { xroot.Add(new XElement(DEFAULTELEMENT)); }
                    XElement target_node = xroot.Elements()?.Last(); //By default get the last node
                    target_node.Add(input_node);
                }
                else
                {
                    xroot.Add(input_node); //if not sub, add to the root
                }
                xdoc.Save(file_name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void write(List<LogBase> memoryData, bool is_sub = false)
        {
            if (memoryData.Count == 0) return; //Don't proceed for empty list
            //If sub, read the xml and get the last node and add everything as sub.
            try
            {
                //Get Xdocument and the root element.
                XDocument xdoc = _getXDocument();
                XElement xroot = _getRoot(xdoc);
                var _input_nodes = ((XElement)convert(memoryData)).Elements();

                if (is_sub)
                {
                    //if sub, find the last node and add to it.
                    if (xroot.Elements().Count() == 0) { xroot.Add(new XElement(DEFAULTELEMENT)); }
                    XElement target_node = xroot.Elements()?.Last(); //By default get the last node
                    target_node.Add(_input_nodes);
                }
                else
                {
                    xroot.Add(_input_nodes); //if not sub, add to the root
                }
                xdoc.Save(file_name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
