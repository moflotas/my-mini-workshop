using BuildingBlocks.Domain;

namespace Modules.UserAccess.Domain.Users.Rules;

public class UserMustHaveInnopolisEmail(string email) : IBusinessRule
{
    public Task<bool> IsBroken()
    {
        return Task.FromResult(!(email.EndsWith("@innopolis.university") || email.EndsWith("@innopolis.ru")));
    }

    public string Message => "User should have email in format @innopolis.university or @innopolis.ru";
}