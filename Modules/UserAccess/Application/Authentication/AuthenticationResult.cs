namespace Modules.UserAccess.Application.Authentication;

public class AuthenticationResult
{
    public bool IsAuthenticated { get; }
    public string? Error { get; }
    public TokensDto? Tokens { get; }
    
    public AuthenticationResult(string authenticationError)
    {
        IsAuthenticated = false;
        Error = authenticationError;
    }

    public AuthenticationResult(TokensDto tokens)
    {
        IsAuthenticated = true;
        Tokens = tokens;
    }
}