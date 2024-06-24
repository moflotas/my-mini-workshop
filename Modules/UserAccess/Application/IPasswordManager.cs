namespace Modules.UserAccess.Application;

public interface IPasswordManager
{
    public string HashPassword(string password);
    public bool VerifyHashedPassword(string hashedPassword, string password);
}