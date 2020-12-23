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

namespace Haley.Utils
{
    public static class ByteConversion
    {
      public static string getPublicKey(this byte[] array)
        {
            try
            {
                var snkpair = new StrongNameKeyPair(array);
                var public_key = snkpair.PublicKey;
                return Convert.ToBase64String(public_key);
            }
            catch (Exception)
            {
                return null;
            }
           
        }
    }
}