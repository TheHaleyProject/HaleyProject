using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security;
using System.Runtime.InteropServices;
using System.Xml;
using Haley.Helpers.Internal;

namespace Haley.Utils
{
    public static class EncryptionUtils
    {
        public sealed class Symmetric
        {
            public static (string key, string salt) encrypt(FileInfo file, string save_path, string _key = null, string _salt = null)
            {
                try
                {
                    if (!file.Exists) return (null, null); //File doesn't exist

                    byte[] _file_bytes = File.ReadAllBytes(file.FullName);
                    var _result = encrypt(_file_bytes, _key, _salt);

                    if (save_path == null)
                    {
                        //Store in same file path with an extension.
                        string _new_name = Path.GetFileNameWithoutExtension(file.FullName) + "_Encrypted";
                        save_path = Path.Combine(file.DirectoryName, _new_name + Path.GetExtension(file.FullName));
                    }

                    //We need to write this data to a file.
                    File.WriteAllBytes(save_path, _result.value); //We either write it to same file or to new filepath.
                    return (Convert.ToBase64String(_result.key), Convert.ToBase64String(_result.salt));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static (string value, string key, string salt) encrypt(string to_encrypt, int key_bits = 512, int salt_bits = 512)
            {
                try
                {
                    byte[] encrypt_byte = Encoding.ASCII.GetBytes(to_encrypt);
                    var result = encrypt(encrypt_byte, key_bits, salt_bits);

                    return (Convert.ToBase64String(result.value), Convert.ToBase64String(result.key), Convert.ToBase64String(result.salt));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static (byte[] value, byte[] key, byte[] salt) encrypt(byte[] to_encrypt, int key_bits = 512, int salt_bits = 512)
            {
                try
                {
                    byte[] _key = HashUtils.getRandomBytes(key_bits).bytes; //Get random key
                    byte[] _iv = HashUtils.getRandomBytes(salt_bits).bytes; //Get random salt

                    return encrypt(to_encrypt, _key, _iv);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static (string value, string key, string salt) encrypt(string to_encrypt, string key, string salt)
            {
                try
                {
                    byte[] encrypt_byte = Encoding.ASCII.GetBytes(to_encrypt);
                    var result = encrypt(encrypt_byte, key, salt);
                    return (Convert.ToBase64String(result.value), Convert.ToBase64String(result.key), Convert.ToBase64String(result.salt));
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            public static (byte[] value, byte[] key, byte[] salt) encrypt(byte[] to_encrypt, string key, string salt)
            {
                try
                {
                    if (key == null && salt == null) return encrypt(to_encrypt);
                    byte[] _key;
                    byte[] _salt;

                    //GET KEY
                    if (key == null)
                    {
                        _key = HashUtils.getRandomBytes(256).bytes;
                    }
                    else if (!key.isBase64())
                    {
                        _key = Encoding.ASCII.GetBytes(key);
                    }
                    else
                    {
                        _key = Convert.FromBase64String(key);
                    }

                    //GET SALT
                    if (salt == null)
                    {
                        _salt = HashUtils.getRandomBytes(256).bytes;
                    }
                    else if (!salt.isBase64())
                    {
                        _salt = Encoding.ASCII.GetBytes(salt);
                    }
                    else
                    {
                        _salt = Convert.FromBase64String(salt);
                    }

                    return encrypt(to_encrypt, _key, _salt);
                }

                catch (Exception)
                {
                    throw;
                }
            }
            public static (byte[] value, byte[] key, byte[] salt) encrypt(byte[] to_encrypt, byte[] _key, byte[] _iv) //Thisis just a proxy method for the internal class
            {
                try
                {
                    return (EncryptionHelper.AES.execute(to_encrypt, _key, _iv, true), _key, _iv);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static string decrypt(string to_decrypt, string key, string iv)
            {
                try
                {
                    if (!to_decrypt.isBase64()) throw new ArgumentException($@"Input not in base 64 format");
                    return Encoding.ASCII.GetString(decrypt(Convert.FromBase64String(to_decrypt), key, iv)); //Because, when decryptd, its not the base 64 byte. Its normal string which is in byte format.
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static void decrypt(FileInfo file, string key, string iv, string save_path)
            {
                try
                {
                    //key, iv, todecrypt: all should be in base 64 format. do a validation to check if the inputs are in base 64 format.

                    if (!file.Exists) throw new ArgumentNullException("File doesn't exist");
                    byte[] to_decrypt = File.ReadAllBytes(file.FullName);
                    var _decryptd_bytes = decrypt(to_decrypt, key, iv);

                    if (save_path == null)
                    {
                        //Store in same file path with an extension.
                        string _new_name = Path.GetFileNameWithoutExtension(file.FullName) + "_DeCrypted";
                        save_path = Path.Combine(file.DirectoryName, _new_name + Path.GetExtension(file.FullName));
                    }

                    File.WriteAllBytes(save_path, _decryptd_bytes);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static byte[] decrypt(byte[] to_decrypt, string key, string iv)
            {
                try
                {
                    byte[] _key;
                    byte[] _iv;

                    //GET KEY
                    if (key.isBase64())
                    {
                        _key = Convert.FromBase64String(key);
                    }
                    else
                    {
                        _key = Encoding.ASCII.GetBytes(key);
                    }

                    //GET IV
                    if (iv.isBase64())
                    {
                        _iv = Convert.FromBase64String(iv);
                    }
                    else
                    {
                        _iv = Encoding.ASCII.GetBytes(iv);
                    }

                    return EncryptionHelper.AES.execute(to_decrypt, _key, _iv, false);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public sealed class ASymmetric
        {
            public static (string public_key, string private_key) getXMLKeyPair()
            {
                try
                {
                    return EncryptionHelper.RSA.getXMLKeyPair();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static (string value, string public_key, string private_key) encrypt(string to_encrypt)
            {
                //User doesn't have any key
                try
                {
                    var kvp = getXMLKeyPair();
                    return (encrypt(to_encrypt, kvp.private_key), kvp.public_key, kvp.private_key);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static string encrypt(string to_encrypt, string public_key)
            {
                //We assume that the public key and private key are already with the software
                try
                {
                    //Asymmetric keys are in xml format.
                    byte[] _to_encrypt_bytes = Encoding.ASCII.GetBytes(to_encrypt);
                    var _encrypted = encrypt(_to_encrypt_bytes, public_key);
                    return Convert.ToBase64String(_encrypted);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static byte[] encrypt(byte[] to_encrypt, string public_key)
            {
                try
                {
                    return EncryptionHelper.RSA.execute(to_encrypt, public_key, true);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static string decrypt(string to_decrypt, string private_key)
            {
                try
                {
                    //The string coming in is a base 64 string.
                    if (!to_decrypt.isBase64()) throw new ArgumentException("Input not in base 64 format");
                    var result = decrypt(Convert.FromBase64String(to_decrypt), private_key);
                    return Encoding.ASCII.GetString(result); //Since result is not in base 64 format.
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static byte[] decrypt(byte[] to_decrypt, string private_key)
            {
                try
                {
                    return EncryptionHelper.RSA.execute(to_decrypt, private_key, false);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public sealed class XML
        {
            public static void sign(XmlDocument input_doc, out XmlDocument output_doc, string _private_key)
            {
                try
                {
                    EncryptionHelper.XML.sign(input_doc, out output_doc, _private_key);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static void verify(XmlDocument input_doc, string _public_key, out bool _status)
            {
                try
                {
                    EncryptionHelper.XML.verify(input_doc, _public_key, out _status);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
