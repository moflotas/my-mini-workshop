namespace BuildingBlocks.Infrastructure.EventBus;

public interface IEventBus : IDisposable
{
    Task Publish<T>(T e)
        where T : IntegrationEvent;

    void Subscribe(IIntegrationEventHandler handler, string eventType);

    void StartConsuming();
}