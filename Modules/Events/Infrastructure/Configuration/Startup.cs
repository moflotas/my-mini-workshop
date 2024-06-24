using System.Reflection;
using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration;
using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.Configuration.EventBus;
using BuildingBlocks.Infrastructure.Configuration.Processing;
using BuildingBlocks.Infrastructure.Configuration.Quartz;
using BuildingBlocks.Infrastructure.EventBus;
using BuildingBlocks.Infrastructure.Inbox;
using BuildingBlocks.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modules.Events.Application.Users;
using Modules.Events.Application.Users.CreateUser;
using Modules.Events.Domain.Users;
using Modules.Events.Infrastructure.Domain.Users;
using Modules.UserAccess.IntegrationEvents;
using Serilog;
using Serilog.Core;

namespace Modules.Events.Infrastructure.Configuration;

public static class Startup
{
    public static IContainer InitEventsModule(Secrets secrets,
        Logger loggerBuilder, IExecutionContextAccessor executionContextAccessor, IEventBus? eventBus = default)
    {
        var builder = new ModuleBuilder(nameof(Events), secrets, loggerBuilder, executionContextAccessor, eventBus);

        return builder
            .RegisterDbContext(options => new EventsContext(options))
            .And()
            .RegisterRepository<UserRepository, IUserRepository>()
            .And()
            .RegisterCommand<CreateUserCommandHandler, CreateUserCommand>()
            .And()
            .RegisterIntegrationEvent<UserCreatedIntegrationEventHandler, UserCreatedIntegrationEvent>()
            .Build();
    }
}