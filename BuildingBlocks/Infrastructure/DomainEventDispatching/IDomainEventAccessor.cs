using BuildingBlocks.Domain;

namespace BuildingBlocks.Infrastructure.DomainEventDispatching;

public interface IDomainEventAccessor
{
    IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

    void ClearAllDomainEvents();
}