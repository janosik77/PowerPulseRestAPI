namespace PowerPulseRestAPI.Security
{
    public interface ISecretProtector
    {
        string Encrypt(string plain);
        string Decrypt(string encrypted);
    }
}
