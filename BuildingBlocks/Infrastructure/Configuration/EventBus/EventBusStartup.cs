using Autofac;
using BuildingBlocks.Infrastructure.EventBus;
using Serilog;

namespace BuildingBlocks.Infrastructure.Configuration.EventBus;

public class EventBusStartup(Func<ILifetimeScope> createScope)
{
    public static EventBusStartup Initialize(Func<ILifetimeScope> createScope)
    {
        return new EventBusStartup(createScope);
    }

    public EventBusStartup Subscribe<T>()
        where T : IntegrationEvent
    {
        using var scope = createScope();
        var eventBus = scope.Resolve<IEventBus>();
        var logger = scope.Resolve<ILogger>();

        logger.Information("Subscribe to {IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(new IntegrationEventGenericHandler<T>(createScope), typeof(T).FullName!);

        return this;
    }
}