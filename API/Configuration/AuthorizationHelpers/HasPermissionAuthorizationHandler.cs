using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration;
using Microsoft.AspNetCore.Authorization;
using Modules.UserAccess.Application.Authorization.GetUserPermissions;
using Modules.UserAccess.Application.Contracts;

namespace API.Configuration.AuthorizationHelpers;

internal class HasPermissionAuthorizationHandler(
    IExecutionContextAccessor executionContextAccessor,
    Settings settings,
    IUserAccessModule userAccessModule)
    : AttributeAuthorizationHandler<HasPermissionAuthorizationRequirement, HasPermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasPermissionAuthorizationRequirement requirement,
        HasPermissionAttribute attribute)
    {
        if (!executionContextAccessor.IsAvailable)
        {
            context.Fail();
            return;
        }

        var userId = executionContextAccessor.UserId;

        if (userId is null)
        {
            context.Fail();
            return;
        }

        var permissions =
            await userAccessModule.ExecuteQueryAsync(new GetUserPermissionsQuery(userId.Value));

        if (!await AuthorizeAsync(attribute.Name, permissions))
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }

    private Task<bool> AuthorizeAsync(string permission, List<UserPermissionDto> permissions)
    {
        return Task.FromResult(settings.IgnorePermissions || permissions.Any(x => x.Code == permission));
    }
}