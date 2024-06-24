namespace BuildingBlocks.Infrastructure.EventBus;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent integrationEvent);
}

public interface IIntegrationEventHandler
{
    Task Handle(IntegrationEvent integrationEvent);
}