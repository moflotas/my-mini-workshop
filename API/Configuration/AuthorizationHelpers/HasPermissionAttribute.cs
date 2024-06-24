using Microsoft.AspNetCore.Authorization;

namespace API.Configuration.AuthorizationHelpers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
internal class HasPermissionAttribute(string name) : AuthorizeAttribute(HasPermissionPolicyName)
{
    public const string HasPermissionPolicyName = "HasPermission";

    public string Name { get; } = name;
}