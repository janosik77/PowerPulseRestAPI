using Microsoft.AspNetCore.DataProtection;

namespace PowerPulseRestAPI.Security
{
    public sealed class SecretProtector : ISecretProtector
    {
        private readonly IDataProtector _protector;

        public SecretProtector(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("PowerPulse.SecretProtector.v1");
        }

        public string Encrypt(string plain)
        {
            if (string.IsNullOrWhiteSpace(plain))
                throw new ArgumentException("Value cannot be null or empty.", nameof(plain));

            return _protector.Protect(plain);
        }

        public string Decrypt(string encrypted)
        {
            if (string.IsNullOrWhiteSpace(encrypted))
                throw new ArgumentException("Value cannot be null or empty.", nameof(encrypted));

            return _protector.Unprotect(encrypted);
        }
    }
}
