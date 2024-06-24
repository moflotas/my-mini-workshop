namespace Modules.UserAccess.Domain.Users;


public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByEmailAsync(string email);
    Task<ICollection<Permission>> GetUserPermissions(Guid userId);
}