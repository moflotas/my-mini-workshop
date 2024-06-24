namespace BuildingBlocks.Infrastructure.DomainEventDispatching;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync();
}