// App_Code/Security/PasswordCrypto.cs (Web Site)
// or Security/PasswordCrypto.cs (Web Application)

using System;
using System.Security.Cryptography;

namespace HelpDesk.Security
{
    /// <summary>
    /// Centralized password hashing utilities (PBKDF2).
    /// Compatible with C# 7.3 (uses classic using(...) blocks).
    /// Matches your DB sizes: Salt=16 bytes, Hash=32 bytes.
    /// </summary>
    public static class PasswordCrypto
    {
        public const int PBKDF2_Iterations = 100_000;
        public const int SaltSize = 16; // varbinary(16)
        public const int HashSize = 32; // varbinary(32)

        public static byte[] CreateSalt()
        {
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // Preferred: requires a framework that supports the HashAlgorithmName overload
        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                PBKDF2_Iterations,
                HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }

        // If your target framework is older and the overload above is missing,
        // uncomment this legacy version and use it instead (PBKDF2-HMAC-SHA1):
        /*
        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, PBKDF2_Iterations))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }
        */

        public static bool VerifyPassword(string password, byte[] salt, byte[] expectedHash)
        {
            var derived = HashPassword(password, salt);
            return FixedTimeEquals(derived, expectedHash);
        }

        // Constant-time comparison to mitigate timing attacks
        public static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
