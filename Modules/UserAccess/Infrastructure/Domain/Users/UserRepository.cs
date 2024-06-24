using Microsoft.EntityFrameworkCore;
using Modules.UserAccess.Domain.Users;

namespace Modules.UserAccess.Infrastructure.Domain.Users;

public class UserRepository(UserAccessContext userAccessContext) : IUserRepository
{
    public async Task AddAsync(User user)
    {
        await userAccessContext.Users.AddAsync(user);
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await userAccessContext.Users.SingleOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await userAccessContext.Users.SingleOrDefaultAsync(x => x.Email == email);
    }

    public async Task<ICollection<Permission>> GetUserPermissions(Guid userId)
    {
        var user = await userAccessContext.Users
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .SingleOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return new List<Permission>();
        }

        return user.Roles.SelectMany(x => x.Permissions).ToList();
    }
}