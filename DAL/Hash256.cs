using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DAL
{
    // Provides salted SHA-256 hashing and verification utilities.
    // Stored format: {saltBase64}:{hexHash}
    public class Hash256
    {
        private static readonly int SaltSize = 16; // 128 bits

        // Create a salted SHA256 hash for the provided password.
        public string CreateSaltedHash(string password)
        {
            if (password == null) password = string.Empty;

            // generate salt
            var saltBytes = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            // compute hash of (salt + password)
            byte[] hashBytes;
            using (var sha = SHA256.Create())
            {
                hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(salt + password));
            }

            // hex encode hash
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("x2"));

            return salt + ":" + sb.ToString();
        }

        // Verify a password against a stored salted hash (format salt:hash)
        public bool Verify(string password, string storedSaltedHash)
        {
            if (storedSaltedHash == null)
                return false;

            var parts = storedSaltedHash.Split(new[] { ':' }, 2);
            if (parts.Length != 2)
                return false;

            string salt = parts[0];
            string expectedHexHash = parts[1];

            // compute hash of (salt + password)
            byte[] hashBytes;
            using (var sha = SHA256.Create())
            {
                hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(salt + (password ?? string.Empty)));
            }

            // convert computed hash to hex
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("x2"));

            string computedHex = sb.ToString();

            // constant time comparison
            return FixedTimeEquals(computedHex, expectedHexHash);
        }

        private bool FixedTimeEquals(string a, string b)
        {
            if (a == null || b == null)
                return false;
            if (a.Length != b.Length)
                return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
