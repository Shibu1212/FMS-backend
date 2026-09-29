using Microsoft.AspNetCore.Identity;
using FormManagementSystem.Models;

namespace FormManagementSystem.Helpers;

public class PasswordHelper
{
    private readonly PasswordHasher<User> _hasher = new();

    public string HashPassword(User user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public bool VerifyPassword(
        User user,
        string passwordHash,
        string password)
    {
        var result = _hasher.VerifyHashedPassword(
            user,
            passwordHash,
            password);

        return result == PasswordVerificationResult.Success;
    }
}