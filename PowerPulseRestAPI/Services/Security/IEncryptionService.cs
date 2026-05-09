namespace PowerPulseRestAPI.Services.Security
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
        string? GetLast4(string? value);
    }
}
