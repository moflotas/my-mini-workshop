using BuildingBlocks.Domain;

namespace Modules.UserAccess.Domain.Users.Events;

public class RoleCreatedDomainEvent(string code) : DomainEventBase
{
    public string Code { get; } = code;
}