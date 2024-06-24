namespace API.Modules.UserAccess.Authentication;

public class AuthenticateRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}