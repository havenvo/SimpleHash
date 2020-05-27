using System;
using System.Security.Cryptography;
using System.Text;

namespace SimpleHash
{
    public class PasswordEncryption
    {
        private const int ROUND = 10;
        
        public string GetHash(string input)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public (string salt, string hashed) HashPassword(string rawPassword)
        {
            var salt = GetHash(GenerateSalt());
            var hashed = GetHash($"{GetHash(rawPassword)}{salt}");
            
            int i = 0;
            while (i < ROUND - 1)
            {
                hashed = GetHash($"{hashed}{salt}");
                i++;
            }
            return (salt, hashed);
        }

        public string GenerateSalt()
        {
            byte[] bytes = new byte[16];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(bytes);
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public bool Verify(string passwordInput, string saltString, string hashedPassword)
        {
            var hashed = GetHash($"{GetHash(passwordInput)}{saltString}");
            int i = 0;
            while (i < ROUND - 1)
            {
                hashed = GetHash($"{hashed}{saltString}");
                i++;
            }
            return string.Equals(hashed, hashedPassword);
        }
    }
}
