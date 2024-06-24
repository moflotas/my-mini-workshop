using BuildingBlocks.Domain;
using MediatR;

namespace BuildingBlocks.Application.Events;

public class DomainEventNotification<T>(Guid id, T domainEvent) : INotification where T : IDomainEvent
{
    public Guid Id { get; } = id;
    public T DomainEvent { get; } = domainEvent;
}