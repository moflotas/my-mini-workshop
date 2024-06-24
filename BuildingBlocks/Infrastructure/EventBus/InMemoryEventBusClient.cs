using Serilog;

namespace BuildingBlocks.Infrastructure.EventBus;

public class InMemoryEventBusClient(ILogger logger) : IEventBus
{
    public async Task Publish<T>(T e)
        where T : IntegrationEvent
    {
        logger.Information("Publishing {Event}", e.GetType().FullName);
        await InMemoryEventBus.Instance.Publish(e);
    }

    public void Subscribe(IIntegrationEventHandler handler, string eventType)
    {
        InMemoryEventBus.Instance.Subscribe(handler, eventType);
    }

    public void StartConsuming()
    {
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}