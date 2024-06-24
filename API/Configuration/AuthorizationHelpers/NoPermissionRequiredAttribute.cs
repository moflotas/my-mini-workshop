namespace API.Configuration.AuthorizationHelpers;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class NoPermissionRequiredAttribute : Attribute;