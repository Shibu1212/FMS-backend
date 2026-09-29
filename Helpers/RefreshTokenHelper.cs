using System.Security.Cryptography;
using System.Text;


namespace FormManagementSystem.Helpers;

public class RefreshTokenHelper
{
    public string Generate()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(randomBytes);
    }

    public string Hash(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);

        var hash = SHA256.HashData(bytes);

        return Convert.ToBase64String(hash);
    }
}