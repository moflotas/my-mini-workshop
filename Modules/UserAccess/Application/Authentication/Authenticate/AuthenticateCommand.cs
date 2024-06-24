using BuildingBlocks.Application.Contracts;

namespace Modules.UserAccess.Application.Authentication.Authenticate;

public class AuthenticateCommand(string email, string password) : CommandBase<AuthenticationResult>
{
    public string Email { get; } = email;

    public string Password { get; } = password;
}