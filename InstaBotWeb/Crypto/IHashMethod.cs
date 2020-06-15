using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuthorize.Crypto
{
    public interface IHashMethod
    {
        public string GetHashCode(HashAlgorithm hashType, string text);
        public bool VerifyHash(HashAlgorithm hashType, string text, string hash);
    }
}
