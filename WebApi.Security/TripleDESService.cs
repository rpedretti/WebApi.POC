using System;
using System.IO;
using System.Security.Cryptography;

namespace WebApi.Security
{
    internal class TripleDESService
    {
        public byte[] GenerateKey()
        {
            using (var tripleDes = TripleDES.Create())
            {
                tripleDes.GenerateKey();
                return tripleDes.Key;
            }
        }

        public byte[] Encrypt(string text, byte[] key)
        {
            using (var tripleDes = TripleDES.Create())
            using (var encryptor = tripleDes.CreateEncryptor(key, tripleDes.IV))
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(text);
                }

                var iv = tripleDes.IV;

                var decryptedContent = msEncrypt.ToArray();

                var result = new byte[iv.Length + decryptedContent.Length];

                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                return result;
            }
        }

        public string Decrypt(byte[] fullCipher, byte[] key)
        {
            var iv = new byte[8];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            using (var tripleDes = TripleDES.Create())
            using (var decryptor = tripleDes.CreateDecryptor(key, iv))
            {
                string result;
                using (var msDecrypt = new MemoryStream(cipher))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    result = srDecrypt.ReadToEnd();
                }

                return result;
            }
        }
    }
}
