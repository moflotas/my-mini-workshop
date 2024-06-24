using BuildingBlocks.Domain;

namespace Modules.UserAccess.Domain.Users.Rules;

public class UserMustHaveUniqueEmail(IUserRepository repository, string email) : IBusinessRule
{
    public async Task<bool> IsBroken()
    {
        return await repository.GetByEmailAsync(email) is not null;
    }

    public string Message => "User with this email already exists";
}