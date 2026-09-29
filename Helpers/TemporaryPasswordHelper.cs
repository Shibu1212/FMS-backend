using System.Security.Cryptography;

namespace FormManagementSystem.Helpers;

public class TemporaryPasswordHelper
{
    public string Generate()
    {
        const string chars =
            "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$%";

        var password = new char[12];

        for (int i = 0; i < password.Length; i++)
        {
            password[i] = chars[
                RandomNumberGenerator.GetInt32(chars.Length)
            ];
        }

        return new string(password);
    }
}