using BuildingBlocks.Application.Contracts;

namespace Modules.UserAccess.Application.Authorization.GetUserPermissions;

public class GetUserPermissionsQuery(Guid userId) : QueryBase<List<UserPermissionDto>>
{
    public Guid UserId { get; } = userId;
}