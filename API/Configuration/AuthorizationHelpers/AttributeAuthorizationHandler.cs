using Microsoft.AspNetCore.Authorization;

namespace API.Configuration.AuthorizationHelpers;

public abstract class AttributeAuthorizationHandler<TRequirement, TAttribute>
    : AuthorizationHandler<TRequirement>
    where TRequirement : IAuthorizationRequirement
    where TAttribute : Attribute
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
    {
        var endpoint = (context.Resource as HttpContext)?.GetEndpoint() as RouteEndpoint;
        var attribute = endpoint?.Metadata.GetMetadata<TAttribute>();

        if (attribute != null) return HandleRequirementAsync(context, requirement, attribute);

        context.Fail();
        return Task.CompletedTask;
    }

    protected abstract Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement,
        TAttribute attribute);
}