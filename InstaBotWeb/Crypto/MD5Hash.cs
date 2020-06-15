using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuthorize.Crypto
{
    public class MD5Hash : IHashMethod
    {
        public string GetHashCode(HashAlgorithm hashType, string text)
        {
            if(string.IsNullOrWhiteSpace(text) && hashType == null)
            {
                throw new ArgumentNullException();
            }

            byte[] data = hashType.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2").ToLower());
            }

            return sBuilder.ToString();
        }

        public bool VerifyHash(HashAlgorithm hashType, string text, string hash)
        {
            string hashOfInput = GetHashCode(hashType, text);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
