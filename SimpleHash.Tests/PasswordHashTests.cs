using System;
using Xunit;

namespace SimpleHash.Tests
{
    public class PasswordHashTests : IDisposable
    {
        private PasswordEncryption passwordEncryption;
       
        public PasswordHashTests()
        {
            passwordEncryption = new PasswordEncryption();
        }

        [Fact]
        public void ShouldHashPasswordSuccess()
        {
            string password = "abc123";
            var result = passwordEncryption.HashPassword(password);

            // Verify that generated salt and the final hash result are not null
            Assert.NotNull(result.salt);
            Assert.NotNull(result.hashed);
        }

        [Fact]
        public void ShouldVerifyPasswordSuccess()
        {
            string password = "abc123";
            var hashedResult = passwordEncryption.HashPassword(password);

            Assert.True(passwordEncryption.Verify(password, hashedResult.salt, hashedResult.hashed));
            Console.WriteLine(hashedResult);
        }

        public void Dispose()
        {
            passwordEncryption = null;
        }
    }
}