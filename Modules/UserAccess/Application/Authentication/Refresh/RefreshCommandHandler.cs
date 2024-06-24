using System.Security.Claims;
using BuildingBlocks.Application.Configuration;
using BuildingBlocks.Application.Configuration.Commands;
using Modules.UserAccess.Domain.RefreshTokens;
using Modules.UserAccess.Domain.Users;

namespace Modules.UserAccess.Application.Authentication.Refresh;

public class RefreshCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtManager jwtManager, Secrets secrets) : ICommandHandler<RefreshCommand, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var claimsPrincipal = jwtManager.CheckJwt(request.AccessToken, false);

        if (claimsPrincipal is null)
        {
            return new AuthenticationResult("Invalid access token");
        }

        var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
        
        if (userIdClaim is null)
        {
            return new AuthenticationResult("Invalid access token");
        }

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new AuthenticationResult("Invalid access token");
        }
        
        var user = await userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return new AuthenticationResult("Invalid access token");
        }
        
        var refreshToken = await refreshTokenRepository.GetByIdAsync(request.RefreshToken);
        
        if (refreshToken is null)
        {
            return new AuthenticationResult("Incorrect refresh token");
        }
        
        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            await refreshTokenRepository.DeleteByIdAsync(refreshToken.Id);
            return new AuthenticationResult("Invalid refresh token");
        }

        await refreshTokenRepository.DeleteByIdAsync(refreshToken.Id);
        
        var newRefreshToken =
            RefreshToken.CreateRefreshToken(user.Id, DateTime.UtcNow.AddMinutes(secrets.Jwt.RefreshExpiresInMinutes));

        await refreshTokenRepository.AddAsync(newRefreshToken);
        
        return new AuthenticationResult(new TokensDto
        {
            AccessToken = jwtManager.GenerateJwt(user),
            RefreshToken = newRefreshToken.Id.ToString()
        });
    }
}