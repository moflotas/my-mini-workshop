using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Domain;
using Modules.UserAccess.Domain.Users.Events;

namespace Modules.UserAccess.Domain.Users;

public class Permission : Entity, IAggregateRoot
{
    [Key] public string Code { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
}