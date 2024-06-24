using BuildingBlocks.Application.Configuration;
using BuildingBlocks.Application.Configuration.Commands;
using Modules.UserAccess.Domain.RefreshTokens;
using Modules.UserAccess.Domain.Users;

namespace Modules.UserAccess.Application.Authentication.Authenticate;

public class AuthenticateCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IPasswordManager passwordManager, IJwtManager jwtManager, Secrets secrets)
    : ICommandHandler<AuthenticateCommand, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            return new AuthenticationResult("Incorrect login or password");
        }

        if (!passwordManager.VerifyHashedPassword(user.Password, request.Password))
        {
            return new AuthenticationResult("Incorrect login or password");
        }

        var refreshToken =
            RefreshToken.CreateRefreshToken(user.Id, DateTime.UtcNow.AddMinutes(secrets.Jwt.RefreshExpiresInMinutes));

        await refreshTokenRepository.AddAsync(refreshToken);
        
        return new AuthenticationResult(new TokensDto
        {
            AccessToken = jwtManager.GenerateJwt(user),
            RefreshToken = refreshToken.Id.ToString()
        });
    }
}