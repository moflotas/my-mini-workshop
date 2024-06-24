using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.EventBus;
using Modules.UserAccess.Application;
using Modules.UserAccess.Application.Authentication;
using Modules.UserAccess.Application.Authentication.Authenticate;
using Modules.UserAccess.Application.Authentication.Refresh;
using Modules.UserAccess.Application.Authorization.GetUserPermissions;
using Modules.UserAccess.Application.Users;
using Modules.UserAccess.Application.Users.CreateUser;
using Modules.UserAccess.Application.Users.EditUser;
using Modules.UserAccess.Domain.RefreshTokens;
using Modules.UserAccess.Domain.Users;
using Modules.UserAccess.Domain.Users.Events;
using Modules.UserAccess.Infrastructure.Domain.RefreshTokens;
using Modules.UserAccess.Infrastructure.Domain.Users;
using Serilog.Core;

namespace Modules.UserAccess.Infrastructure.Configuration;

public static class Startup
{
    public static IContainer InitUserAccessModule(Secrets secrets,
        Logger loggerBuilder, IExecutionContextAccessor executionContextAccessor, IEventBus? eventBus = default)
    {
        var builder = new ModuleBuilder(nameof(UserAccess), secrets, loggerBuilder, executionContextAccessor, eventBus);

        return builder
            .RegisterDbContext(options => new UserAccessContext(options))
            .And()
            .RegisterRepository<UserRepository, IUserRepository>()
            .RegisterRepository<RefreshTokenRepository, IRefreshTokenRepository>()
            .And()
            .RegisterCommandWithResult<AuthenticateCommandHandler, AuthenticateCommand, AuthenticationResult>()
            .RegisterCommandWithResult<RefreshCommandHandler, RefreshCommand, AuthenticationResult>()
            .RegisterCommandWithResult<CreateUserCommandHandler, CreateUserCommand, Guid>()
            .RegisterCommand<EditUserCommandHandler, EditUserCommand>()
            .RegisterQuery<GetUserPermissionsQueryHandler, GetUserPermissionsQuery, List<UserPermissionDto>>()
            .RegisterNotification<UserCreatedNotificationHandler, UserCreatedDomainEvent>()
            .And()
            .RegisterSingleton<PasswordManager, IPasswordManager>()
            .RegisterSingleton<JwtManager, IJwtManager>()
            .Build();
    }
}