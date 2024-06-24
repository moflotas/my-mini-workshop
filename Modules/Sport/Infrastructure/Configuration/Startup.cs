using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.EventBus;
using Modules.Sport.Application.Users;
using Modules.Sport.Application.Users.CreateUser;
using Modules.Sport.Domain.Users;
using Modules.Sport.Infrastructure.Domain.Users;
using Modules.UserAccess.IntegrationEvents;
using Serilog.Core;

namespace Modules.Sport.Infrastructure.Configuration;

public static class Startup
{
    public static IContainer InitSportModule(Secrets secrets,
        Logger loggerBuilder, IExecutionContextAccessor executionContextAccessor, IEventBus? eventBus = default)
    {
        var builder = new ModuleBuilder(nameof(Sport), secrets, loggerBuilder, executionContextAccessor, eventBus);

        return builder
            .RegisterDbContext(options => new SportContext(options))
            .And()
            .RegisterRepository<UserRepository, IUserRepository>()
            .And()
            .RegisterCommand<CreateUserCommandHandler, CreateUserCommand>()
            .And()
            .RegisterIntegrationEvent<UserCreatedIntegrationEventHandler, UserCreatedIntegrationEvent>()
            .Build();
    }
}