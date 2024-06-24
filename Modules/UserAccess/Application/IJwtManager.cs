using System.Security.Claims;
using Modules.UserAccess.Domain.Users;

namespace Modules.UserAccess.Application;

public interface IJwtManager
{
    public string GenerateJwt(User user);
    public ClaimsPrincipal? CheckJwt(string token, bool validateLifetime = true);
}