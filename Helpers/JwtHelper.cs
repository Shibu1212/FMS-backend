using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FormManagementSystem.Models;
using Microsoft.IdentityModel.Tokens;

namespace FormManagementSystem.Helpers;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(User user)
    {
        var key = _configuration["Jwt:Key"];

        var issuer = _configuration["Jwt:Issuer"];

        var audience = _configuration["Jwt:Audience"];

        var expiryMinutes =
            _configuration.GetValue<int>(
                "Jwt:AccessTokenExpiryMinutes");

        var roleName = user.UserRoles?.FirstOrDefault()?.Role?.Name;

        if (string.IsNullOrEmpty(roleName))
        {
            throw new InvalidOperationException($"User '{user.Email}' does not have an assigned role.");
        }

        var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.PublicId.ToString()),

                new Claim(
                    "name",
                    user.Name),

                new Claim(
                    "email",
                    user.Email),

                new Claim(
                    "role",
                    roleName)
            };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key!));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                expiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}