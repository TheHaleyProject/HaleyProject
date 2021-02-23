using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using CryptXML = System.Security.Cryptography.Xml;
using System.IO;
using System.Xml;

namespace Haley.Helpers.Internal
{
    internal sealed class EncryptionHelper
    {
        internal sealed class AES
        {
            public static byte[] execute(byte[] to_execute, byte[] key, byte[] iv, bool is_encrypt)
            {
                try
                {
                    using (RijndaelManaged rjManaged = new RijndaelManaged())
                    {
                        rjManaged.Padding = PaddingMode.PKCS7;
                        rjManaged.Mode = CipherMode.CBC;

                        var combinedkey = new Rfc2898DeriveBytes(key, iv, 100);
                        var _new_key = combinedkey.GetBytes(rjManaged.KeySize / 8);
                        var _new_iv = combinedkey.GetBytes(rjManaged.BlockSize / 8);

                        using (MemoryStream mstream = new MemoryStream())
                        {
                            ICryptoTransform cryptor = null;

                            //Based on the method, we will either create a encryptor or decryptor using the key and iv
                            switch (is_encrypt)
                            {
                                case true: //Then write the stream using an encryptor
                                    cryptor = rjManaged.CreateEncryptor(_new_key, _new_iv); //Encryptor
                                    break;

                                case false: //Then write the stream using a decryptor.
                                    cryptor = rjManaged.CreateDecryptor(_new_key, _new_iv); //Decryptor
                                    break;
                            }

                            using (CryptoStream cstream = new CryptoStream(mstream, cryptor, CryptoStreamMode.Write))
                            {
                                cstream.Write(to_execute, 0, to_execute.Length);
                                cstream.FlushFinalBlock(); //We are using specified length of key and salt to encrypt. So the last block might not be perfect. It can be empty. So, we are flushing it.
                            }

                            //Instead of above method, we can also use a method where we load the memory stream with the byte array of the encrypted text. Then the cryptostream will be reading the memory stream and return the results.
                            var result = mstream.ToArray();
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        internal sealed class RSA
        {
            public static (string public_key, string private_key) getXMLKeyPair()
            {
                try
                {
                    var rsa_provider = new RSACryptoServiceProvider(1024);
                    return (rsa_provider.ToXmlString(false), rsa_provider.ToXmlString(true));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static byte[] execute(byte[] to_execute, string _key, bool is_encrypt)
            {
                //IMPORTANT: PUBLIC KEY IS FOR ENCRYPTION AND PRIVATE KEY IS FOR DECRYPTION.
                var rsa_provider = new RSACryptoServiceProvider();
                try
                {
                    rsa_provider.FromXmlString(_key);
                    var padding = RSAEncryptionPadding.OaepSHA1;
                    switch (is_encrypt)
                    {
                        case true:
                            return rsa_provider.Encrypt(to_execute, padding);
                        case false:
                            return rsa_provider.Decrypt(to_execute, padding);
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    rsa_provider.Dispose();
                }
            }
        }
        internal sealed class XML
        {
            public static void sign(XmlDocument input_doc, out XmlDocument output_doc, string private_key)
            {
                try
                {
                    //Setup RSA provider using the provided private key
                    var rsa_provider = new RSACryptoServiceProvider();
                    rsa_provider.FromXmlString(private_key);

                    //Create a temporary xml based on the input XML. Add the Signing key created in previous step.
                    CryptXML.SignedXml _temporary_xml = new CryptXML.SignedXml(input_doc);
                    _temporary_xml.SigningKey = rsa_provider;

                    //Reference is indicating what to sign inside the XML. In our case, we need the whole xml document to sign. So set the URI as ""
                    CryptXML.Reference _signing_reference = new CryptXML.Reference();
                    _signing_reference.Uri = "";

                    //Create a verification object that can be stored inside the XMl, so that it can be verified later using the public key. Very vital step or else the verification will not be done and whole purpose of signing is defied.
                    var _verification_transform = new CryptXML.XmlDsigEnvelopedSignatureTransform();

                    //Set the verification object inside the reference object.
                    _signing_reference.AddTransform(_verification_transform);

                    //Add this reference to the XML
                    _temporary_xml.AddReference(_signing_reference);

                    //Compute
                    _temporary_xml.ComputeSignature();

                    //So far all steps are done in a temporary holder. Get the signature from there.
                    XmlElement _signature_element = _temporary_xml.GetXml();

                    output_doc = input_doc;
                    //Finally add this element to the input xml.
                    output_doc.DocumentElement.AppendChild(_signature_element);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static void verify(XmlDocument input_doc, string public_key, out bool _status)
            {
                try
                {
                    _status = false;

                    //Create RSA Provider from Public Key
                    var rsa_provider = new RSACryptoServiceProvider(1024);
                    rsa_provider.FromXmlString(public_key);

                    //Create temporary xml reference using the input xml document
                    var temp_xml = new CryptXML.SignedXml(input_doc);

                    //Get the signature node for verification
                    var node_list = input_doc.GetElementsByTagName("Signature");

                    //Check if only one signature is present. If so, add it to the temporaryxml
                    if (node_list.Count != 1) return;
                    XmlElement signature_element = node_list[0] as XmlElement;
                    if (signature_element == null) return;
                    temp_xml.LoadXml(signature_element);

                    //Validate the signature
                    _status = temp_xml.CheckSignature(rsa_provider);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

