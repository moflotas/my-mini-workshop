using BuildingBlocks.Infrastructure.EventBus;

namespace Modules.UserAccess.IntegrationEvents;

public class UserCreatedIntegrationEvent(
    Guid id,
    DateTime occurredOn,
    Guid userId,
    string email,
    string? firstNameRu,
    string? lastNameRu,
    string? patronymicRu,
    string? firstNameEn,
    string? lastNameEn,
    string? patronymicEn)
    : IntegrationEvent(id, occurredOn)
{
    public Guid UserId { get; } = userId;
    public string Email { get; } = email;

    public string? FirstNameRu { get; set; } = firstNameRu;
    public string? LastNameRu { get; set; } = lastNameRu;
    public string? PatronymicRu { get; set; } = patronymicRu;

    public string? FirstNameEn { get; set; } = firstNameEn;
    public string? LastNameEn { get; set; } = lastNameEn;
    public string? PatronymicEn { get; set; } = patronymicEn;
}