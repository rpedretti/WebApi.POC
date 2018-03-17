using System;
using System.Security.Cryptography;
using System.Text;
using WebApi.Security.Extensions;

namespace WebApi.Security
{
    internal class RSAService
    {
        public Tuple<string, string> GenerateKeyPair()
        {
            using (var rsa = RSA.Create())
            {
                var publicKey = rsa.ToXmlString();
                var publicPrivate = rsa.ToXmlString(true);

                return Tuple.Create(publicKey, publicPrivate);
            }
        }

        public byte[] Encrypt(string value, string key)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(key);
                return rsa.Encrypt(Encoding.UTF8.GetBytes(value), RSAEncryptionPadding.Pkcs1);
            }
        }

        public string Decrypt(byte[] value, string key)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(key);
                return Encoding.UTF8.GetString(rsa.Decrypt(value, RSAEncryptionPadding.Pkcs1));
            }
        }
    }
}
