using BuildingBlocks.Application.Contracts;

namespace Modules.UserAccess.Application.Authentication.Refresh;

public class RefreshCommand(Guid refreshToken, string accessToken) : CommandBase<AuthenticationResult>
{
    public Guid RefreshToken = refreshToken;
    
    public string AccessToken = accessToken;
}