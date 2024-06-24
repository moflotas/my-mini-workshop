using Microsoft.EntityFrameworkCore;
using Modules.Events.Domain.Users;
using Modules.Events.Infrastructure;

namespace Modules.Events.Infrastructure.Domain.Users;

public class UserRepository(EventsContext sportContext) : IUserRepository
{
    public async Task AddAsync(User user)
    {
        await sportContext.Users.AddAsync(user);
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await sportContext.Users.SingleOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await sportContext.Users.SingleOrDefaultAsync(x => x.Email == email);
    }
}