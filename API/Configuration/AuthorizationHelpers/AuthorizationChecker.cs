using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace API.Configuration.AuthorizationHelpers;

public static class AuthorizationChecker
{
    public static void CheckAllEndpoints()
    {
        var assembly = typeof(Program).Assembly;
        var allControllerTypes = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(ControllerBase)));

        List<string> notProtectedActionMethods = [];
        foreach (var controllerType in allControllerTypes)
        {
            var controllerHasPermissionAttribute = controllerType.GetCustomAttribute<HasPermissionAttribute>();
            if (controllerHasPermissionAttribute != null)
            {
                continue;
            }

            var actionMethods = controllerType.GetMethods()
                .Where(x => x.IsPublic && x.DeclaringType == controllerType)
                .ToList();

            foreach (var publicMethod in actionMethods)
            {
                var hasPermissionAttribute = publicMethod.GetCustomAttribute<HasPermissionAttribute>();
                if (hasPermissionAttribute != null) continue;

                var noPermissionRequired = publicMethod.GetCustomAttribute<NoPermissionRequiredAttribute>();

                if (noPermissionRequired != null) continue;

                notProtectedActionMethods.Add($"{controllerType.Name}.{publicMethod.Name}");
            }
        }

        if (notProtectedActionMethods.Count == 0) return;

        var errorBuilder = new StringBuilder();
        errorBuilder.AppendLine("Invalid authorization configuration: ");

        foreach (var notProtectedActionMethod in notProtectedActionMethods)
        {
            errorBuilder.AppendLine($"Method {notProtectedActionMethod} is not protected. ");
        }

        throw new ApplicationException(errorBuilder.ToString());
    }
}