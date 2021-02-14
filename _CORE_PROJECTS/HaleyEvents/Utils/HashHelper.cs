using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security;
using System.Runtime.InteropServices;

namespace Haley.Events.Utils
{
    internal class HashHelper
    {
        public static byte[] computeHash(string to_hash)
        {
            byte[] computed_hash = null;
            var _to_hash_bytes = Encoding.ASCII.GetBytes(to_hash);
            using (var cryptoProvider = new MD5CryptoServiceProvider())
            {
                computed_hash = cryptoProvider.ComputeHash(_to_hash_bytes);
            }
            return computed_hash;
        }
    }
}
