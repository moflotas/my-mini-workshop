using DevOne.Security.Cryptography.BCrypt;
using Modules.UserAccess.Application;

namespace Modules.UserAccess.Infrastructure;

public class PasswordManager : IPasswordManager
{
    public string HashPassword(string password)
    {
        var salt = BCryptHelper.GenerateSalt();
        return BCryptHelper.HashPassword(password, salt);
    }

    public bool VerifyHashedPassword(string hashedPassword, string password)
    {
        return BCryptHelper.CheckPassword(password, hashedPassword);
    }
}