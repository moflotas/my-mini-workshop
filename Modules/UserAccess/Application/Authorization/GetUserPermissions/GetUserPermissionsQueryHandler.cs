using BuildingBlocks.Application.Configuration.Queries;
using Modules.UserAccess.Domain.Users;

namespace Modules.UserAccess.Application.Authorization.GetUserPermissions;

public class GetUserPermissionsQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserPermissionsQuery, List<UserPermissionDto>>
{
    public async Task<List<UserPermissionDto>> Handle(GetUserPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        return (await userRepository.GetUserPermissions(request.UserId))
            .Select(p => new UserPermissionDto { Code = p.Code })
            .ToList();
    }
}