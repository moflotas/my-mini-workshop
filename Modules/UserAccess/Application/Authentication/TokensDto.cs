namespace Modules.UserAccess.Application.Authentication;

public class TokensDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}