using Microsoft.EntityFrameworkCore;
using Modules.UserAccess.Domain.RefreshTokens;

namespace Modules.UserAccess.Infrastructure.Domain.RefreshTokens;

public class RefreshTokenRepository(UserAccessContext userAccessContext) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken token)
    {
        await userAccessContext.RefreshTokens.AddAsync(token);
    }

    public async Task<RefreshToken?> GetByIdAsync(Guid tokenId)
    {
        return await userAccessContext.RefreshTokens.SingleOrDefaultAsync(x => x.Id == tokenId);
    }

    public async Task DeleteByIdAsync(Guid tokenId)
    {
        var token = await userAccessContext.RefreshTokens.SingleOrDefaultAsync(x => x.Id == tokenId);
        
        if (token is null)
        {
            return;
        }

        userAccessContext.Remove(token);
    }
}