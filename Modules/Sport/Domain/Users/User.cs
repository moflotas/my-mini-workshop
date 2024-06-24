using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Domain;
using Modules.Sport.Domain.Users.Events;

namespace Modules.Sport.Domain.Users;

public class User : Entity, IAggregateRoot
{
    [Key] public Guid Id { get; set; }
    public string Email { get; set; } = default!;

    public string? FirstNameRu { get; set; }
    public string? LastNameRu { get; set; }
    public string? PatronymicRu { get; set; }

    public string? FirstNameEn { get; set; }
    public string? LastNameEn { get; set; }
    public string? PatronymicEn { get; set; }
    
    // For EF Core //
    private User()
    {
    }

    public static Task<User> CreateUser(
        Guid userId,
        string email,
        string? firstNameRu,
        string? lastNameRu,
        string? patronymicRu,
        string? firstNameEn,
        string? lastNameEn,
        string? patronymicEn
    )
    {
        return Task.FromResult(new User(
            userId,
            email,
            firstNameRu,
            lastNameRu,
            patronymicRu,
            firstNameEn,
            lastNameEn,
            patronymicEn
        ));
    }

    private User(
        Guid id,
        string email,
        string? firstNameRu,
        string? lastNameRu,
        string? patronymicRu,
        string? firstNameEn,
        string? lastNameEn,
        string? patronymicEn)
    {
        Id = id;
        Email = email;

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