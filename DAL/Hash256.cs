using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DAL
{
    // PBKDF2 password hashing utility.
    // Stored format: pbkdf2$iterations$saltBase64$hashBase64
    public class Hash256
    {
        private const int SaltSize = 16;
        private const int HashSize = 32; // 256 bits
        private const int Iterations = 10000;

        // Create PBKDF2 hash
        public string CreateHash(string password)
        {
            if (password == null) password = string.Empty;
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                hash = pbkdf2.GetBytes(HashSize);
            }
            return string.Format("pbkdf2${0}${1}${2}", Iterations, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        // Verify password against stored hash. Supports legacy hex SHA256 as fallback (caller may migrate).
        public bool Verify(string password, string storedHash)
        {
            if (storedHash == null) return false;
            if (storedHash.StartsWith("pbkdf2$"))
            {
                try
                {
                    var parts = storedHash.Split('$');
                    if (parts.Length != 4) return false;

                    // helper to trim surrounding whitespace, quotes, and parentheses
                    string Clean(string s)
                    {
                        if (s == null) return string.Empty;
                        return s.Trim().Trim('"', '\'', '(', ')');
                    }

                    var iterPart = Clean(parts[1]);
                    if (!int.TryParse(iterPart, out int iter) || iter <= 0) return false;

                    var saltPart = Clean(parts[2]);
                    var hashPart = Clean(parts[3]);

                    byte[] salt = Convert.FromBase64String(saltPart);
                    byte[] expected = Convert.FromBase64String(hashPart);
                    byte[] actual;
                    using (var pbkdf2 = new Rfc2898DeriveBytes(password ?? string.Empty, salt, iter))
                    {
                        actual = pbkdf2.GetBytes(expected.Length);
                    }
                    return FixedTimeEquals(actual, expected);
                }
                catch (FormatException)
                {
                    return false;
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }

            // legacy hex SHA256 stored; caller should handle migration if VerifyLegacy returns true
            return false;
        }

        // Verify legacy unsalted hex SHA256
        public bool VerifyLegacySha256(string password, string legacyHexHash)
        {
            if (legacyHexHash == null) return false;
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password ?? string.Empty));
                var sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) sb.Append(bytes[i].ToString("x2"));
                return StringComparer.OrdinalIgnoreCase.Compare(sb.ToString(), legacyHexHash) == 0;
            }
        }

        private bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++) diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
