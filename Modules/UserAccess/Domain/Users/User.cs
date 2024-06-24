using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Domain;
using Modules.UserAccess.Domain.Users.Events;
using Modules.UserAccess.Domain.Users.Rules;

namespace Modules.UserAccess.Domain.Users;

public class User : Entity, IAggregateRoot
{
    [Key] public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    public string? FirstNameRu { get; set; }
    public string? LastNameRu { get; set; }
    public string? PatronymicRu { get; set; }

    public string? FirstNameEn { get; set; }
    public string? LastNameEn { get; set; }
    public string? PatronymicEn { get; set; }

    public ICollection<Role> Roles { get; set; } = new List<Role>();

    // For EF Core //
    private User()
    {
    }
    
    public static async Task<User> CreateUser(
        Guid userId,
        string email,
        string password,
        string? firstNameRu,
        string? lastNameRu,
        string? patronymicRu,
        string? firstNameEn,
        string? lastNameEn,
        string? patronymicEn,
        IUserRepository userRepository
    )
    {
        await CheckRule(new UserMustHaveInnopolisEmail(email));
        await CheckRule(new UserMustHaveUniqueEmail(userRepository, email));

        return new User(
            userId,
            email,
            password,
            firstNameRu,
            lastNameRu,
            patronymicRu,
            firstNameEn,
            lastNameEn,
            patronymicEn
        );
    }

    private User(
        Guid id,
        string email,
        string password,
        string? firstNameRu,
        string? lastNameRu,
        string? patronymicRu,
        string? firstNameEn,
        string? lastNameEn,
        string? patronymicEn)
    {
        Id = id;
        Email = email;
        Password = password;

        FirstNameRu = firstNameRu;
        LastNameRu = lastNameRu;
        PatronymicRu = patronymicRu;
        FirstNameEn = firstNameEn;
        LastNameEn = lastNameEn;
        PatronymicEn = patronymicEn;

        AddDomainEvent(new UserCreatedDomainEvent(id, email, firstNameRu, lastNameRu, patronymicRu, firstNameEn,
            lastNameEn, patronymicEn
        ));
    }
}