using API.Configuration.AuthorizationHelpers;
using Microsoft.AspNetCore.Authorization;

namespace API.Configuration;

public static class Authorization
{
    public static void InitAuthorization(this IServiceCollection s)
    {
        AuthorizationChecker.CheckAllEndpoints();

        s.AddAuthorizationBuilder()
            .AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
            {
                policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
                policyBuilder.AddAuthenticationSchemes("Bearer");
            });

        s.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
    }
}