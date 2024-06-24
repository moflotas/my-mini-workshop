using System.Reflection;
using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration;
using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Application.Configuration.Queries;
using BuildingBlocks.Application.Contracts;
using BuildingBlocks.Application.Events;
using BuildingBlocks.Domain;
using BuildingBlocks.Infrastructure.Configuration.Database;
using BuildingBlocks.Infrastructure.Configuration.EventBus;
using BuildingBlocks.Infrastructure.Configuration.Processing;
using BuildingBlocks.Infrastructure.Configuration.Quartz;
using BuildingBlocks.Infrastructure.EventBus;
using BuildingBlocks.Infrastructure.Inbox;
using BuildingBlocks.Infrastructure.InternalCommands;
using BuildingBlocks.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;

namespace BuildingBlocks.Infrastructure.Configuration;

public class ModuleBuilder
{
    private readonly ContainerBuilder _builder = new();
    private readonly string _moduleName;
    private readonly Secrets _secrets;
    private readonly Logger _loggerBuilder;
    private readonly List<Action<EventBusStartup>> _events = [];

    public ModuleBuilder(
        string moduleName,
        Secrets secrets,
        Logger loggerBuilder,
        IExecutionContextAccessor executionContextAccessor,
        IEventBus? eventBus)
    {
        _moduleName = moduleName;
        _secrets = secrets;
        _loggerBuilder = loggerBuilder;

        _builder.RegisterInstance(executionContextAccessor);
        _builder.RegisterInstance(secrets);
        _builder.RegisterModule<ProcessingModule>();
        _builder.RegisterModule(new EventBusModule(eventBus));

        RegisterServiceProviderWrapper();
        RegisterMediator();
        RegisterLogger();
        RegisterBoxes();
    }


    private ModuleBuilder RegisterServiceProviderWrapper()
    {
        _builder.RegisterType<ServiceProviderWrapper>()
            .As<IServiceProvider>()
            .InstancePerDependency()
            .IfNotRegistered(typeof(IServiceProvider));
        return this;
    }

    private ModuleBuilder RegisterMediator()
    {
        _builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        return this;
    }

    private ModuleBuilder RegisterLogger()
    {
        _builder.RegisterInstance(_loggerBuilder.ForContext("Module", _moduleName))
            .As<ILogger>()
            .SingleInstance();
        return this;
    }

    private ModuleBuilder RegisterBoxes()
    {
        _builder.RegisterType<OutboxAccessor>()
            .As<IOutbox>()
            .InstancePerLifetimeScope();

        RegisterCommand<ProcessInboxCommandHandler, ProcessInboxCommand>();
        RegisterCommand<ProcessOutboxCommandHandler, ProcessOutboxCommand>();
        RegisterCommand<ProcessInternalCommandHandler, ProcessInternalCommand>();

        _builder.RegisterType<ProcessInboxJob>()
            .AsSelf()
            .InstancePerDependency();
        _builder.RegisterType<ProcessOutboxJob>()
            .AsSelf()
            .InstancePerDependency();
        _builder.RegisterType<ProcessInternalJob>()
            .AsSelf()
            .InstancePerDependency();

        return this;
    }

    public ModuleBuilder RegisterDbContext<T>(Func<DbContextOptions, T> contextFactory) where T : AppDbContext
    {
        _builder.Register(_ => contextFactory(new DbContextOptionsBuilder()
                .UseNpgsql(_secrets.Database.ConnectionString)
                .Options))
            .AsSelf()
            .As<DbContext>()
            .As<IAppDbContext>()
            .InstancePerLifetimeScope();

        return this;
    }

    public ModuleBuilder RegisterRepository<T, TService>() where T : class where TService : notnull
    {
        _builder.RegisterType<T>()
            .As<TService>()
            .InstancePerLifetimeScope();
        return this;
    }

    public ModuleBuilder RegisterCommand<TCommandHandler, TCommand>()
        where TCommandHandler : class where TCommand : CommandBase
    {
        _builder.RegisterType<TCommandHandler>()
            .As<ICommandHandler<TCommand>>()
            .As<IRequestHandler<TCommand>>()
            .InstancePerLifetimeScope();
        return this;
    }

    public ModuleBuilder RegisterCommandWithResult<TCommandHandler, TCommand, TResult>() where TCommandHandler : class
        where TCommand : CommandBase<TResult>
    {
        _builder.RegisterType<TCommandHandler>()
            .As<ICommandHandler<TCommand, TResult>>()
            .As<IRequestHandler<TCommand, TResult>>()
            .InstancePerLifetimeScope();
        return this;
    }

    public ModuleBuilder RegisterQuery<TQueryHandler, TQuery, TResult>()
        where TQueryHandler : class where TQuery : QueryBase<TResult>
    {
        _builder.RegisterType<TQueryHandler>()
            .As<IQueryHandler<TQuery, TResult>>()
            .As<IRequestHandler<TQuery, TResult>>()
            .InstancePerLifetimeScope();
        return this;
    }

    public ModuleBuilder RegisterNotification<TNotificationHandler, TNotification>() where TNotificationHandler : class
        where TNotification : DomainEventBase
    {
        _builder.RegisterType<TNotificationHandler>()
            .As<INotificationHandler<DomainEventNotification<TNotification>>>()
            .InstancePerLifetimeScope();
        return this;
    }

    public ModuleBuilder RegisterIntegrationEvent<TIntegrationEventHandler, TIntegrationEvent>()
        where TIntegrationEventHandler : INotificationHandler<TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        _builder.RegisterType<TIntegrationEventHandler>()
            .As<INotificationHandler<TIntegrationEvent>>()
            .InstancePerLifetimeScope();
        _events.Add(Subscribe);

        return this;

        void Subscribe(EventBusStartup startup)
        {
            startup.Subscribe<TIntegrationEvent>();
        }
    }

    public ModuleBuilder RegisterSingleton<T, TService>() where T : class where TService : notnull
    {
        _builder.RegisterType<T>()
            .As<TService>()
            .SingleInstance();
        return this;
    }

    public ModuleBuilder And()
    {
        return this;
    }

    public IContainer Build()
    {
        var container = _builder.Build();
        var createScope = () => container.BeginLifetimeScope();

        DatabaseStartup.Initialize(createScope);

        var eventBus = EventBusStartup.Initialize(createScope);
        _events.ForEach(integrationEvent => integrationEvent(eventBus));

        QuartzStartup.Initialize(createScope, _moduleName);

        return container;
    }
}