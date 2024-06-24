using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuildingBlocks.Application.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modules.UserAccess.Application;
using Modules.UserAccess.Domain.Users;

namespace Modules.UserAccess.Infrastructure;

public class JwtManager(Secrets secrets) : IJwtManager
{
    public string GenerateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email)
        };
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Code)));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrets.Jwt.Key));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: secrets.Jwt.Issuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(secrets.Jwt.ExpiresInMinutes),
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? CheckJwt(string token, bool validateLifetime = true)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrets.Jwt.Key)),
            ValidIssuer = secrets.Jwt.Issuer,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
        
        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }
}