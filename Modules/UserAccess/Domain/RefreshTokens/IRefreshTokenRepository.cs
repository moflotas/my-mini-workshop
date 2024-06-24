namespace Modules.UserAccess.Domain.RefreshTokens;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByIdAsync(Guid tokenId);
    Task DeleteByIdAsync(Guid tokenId);

}