using System.Security.Cryptography;
using System.Text;

namespace FormManagementSystem.Helpers;

public class PasswordResetTokenHelper
{
    public string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes);
    }

    public string HashToken(string token)
    {
        using var sha256 = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha256.ComputeHash(bytes);

        return Convert.ToHexString(hash);
    }
}