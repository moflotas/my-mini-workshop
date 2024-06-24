using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Domain;
using Modules.UserAccess.Domain.Users.Events;

namespace Modules.UserAccess.Domain.Users;

public class Role : Entity, IAggregateRoot
{
    [Key] public string Code { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public ICollection<Permission> Permissions { get; } = new List<Permission>();

    // For EF Core
    private Role()
    {
    }

    private Role(
        string code,
        string displayName)
    {
        Code = code;
        DisplayName = displayName;

        AddDomainEvent(new RoleCreatedDomainEvent(code));
    }
}