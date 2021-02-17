using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SearchIcd10.DataProviders
{
    /// <summary>
    /// Class used to encrypt Session data for the NMS Services
    /// </summary>
    internal class SessionDataEncryptor
    {
        /// <summary>
        /// Key used to encrypt Session information
        /// </summary>
        private static readonly string EncryptKey = "" /* Need an actual key here! */;
        /// <summary>
        /// IV used to encrypt Session information
        /// </summary>
        private static readonly string EncryptIv = "" /* Need an actual key here! */;

        /// <summary>
        /// Encrypt a given string using AES encryption and a default Key and IV
        /// </summary>
        /// <param name="inStr">String to encrypt</param>
        /// <returns>Base64 encoded, encrypted string</returns>
        public string Encrypt(string inStr)
        {
            var key = Encoding.Default.GetBytes(EncryptKey);
            if (key.Length > 32)
            {
                key = key.Take(32).ToArray();
            }
            var iv = Encoding.Default.GetBytes(EncryptIv);
            if (iv.Length > 16)
            {
                iv = iv.Take(16).ToArray();
            }

            string result = String.Empty;

            var rijn = new RijndaelManaged();
            using (var encryptor = rijn.CreateEncryptor(key, iv))
            {
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        var bytes = Encoding.Default.GetBytes(inStr);
                        cs.Write(bytes, 0, bytes.Length);
                    }
                    result = Convert.ToBase64String(ms.ToArray());
                }
            }
            rijn.Clear();

            return result;
        }

        /// <summary>
        /// Decrypt a given Base64 encoded string that has been encrypted via AES with the default Key and IV
        /// </summary>
        /// <param name="inStr">Base64, Encrypted string to decrypt</param>
        /// <returns>The unencrypted string to decrypt</returns>
        public string Decrypt(string inStr)
        {
            var key = Encoding.Default.GetBytes(EncryptKey);
            if (key.Length > 32)
            {
                key = key.Take(32).ToArray();
            }
            var iv = Encoding.Default.GetBytes(EncryptIv);
            if (iv.Length > 16)
            {
                iv = iv.Take(16).ToArray();
            }

            string result = String.Empty;

            var rijn = new RijndaelManaged();
            using (var decryptor = rijn.CreateDecryptor(key, iv))
            {
                using (var ms = new MemoryStream(Convert.FromBase64String(inStr)))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }
            rijn.Clear();

            return result;
        }
    }
}
