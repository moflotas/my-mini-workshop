using BuildingBlocks.Domain;

namespace Modules.Sport.Domain.Users.Events;

public class UserCreatedDomainEvent(
    Guid userId,
    string email,
    string? firstNameRu,
    string? lastNameRu,
    string? patronymicRu,
    string? firstNameEn,
    string? lastNameEn,
    string? patronymicEn) : DomainEventBase
{
    public Guid UserId { get; } = userId;
    public string Email { get; } = email;
    public string? FirstNameRu { get; } = firstNameRu;
    public string? LastNameRu { get; } = lastNameRu;
    public string? PatronymicRu { get; } = patronymicRu;
    public string? FirstNameEn { get; } = firstNameEn;
    public string? LastNameEn { get; } = lastNameEn;
    public string? PatronymicEn { get; } = patronymicEn;
}