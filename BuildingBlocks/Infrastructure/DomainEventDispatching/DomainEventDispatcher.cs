using System.Text.Json;
using BuildingBlocks.Application.Events;
using BuildingBlocks.Domain;
using MediatR;

namespace BuildingBlocks.Infrastructure.DomainEventDispatching;

public class DomainEventDispatcher(
    IMediator mediator,
    IDomainEventAccessor domainEventsProvider)
    : IDomainEventDispatcher
{
    public async Task DispatchEventsAsync()
    {
        var domainEvents = domainEventsProvider.GetAllDomainEvents();

        domainEventsProvider.ClearAllDomainEvents();

        foreach (var notification in domainEvents.Select(CreateDomainEventNotification))
        {
            await mediator.Publish(notification);
        }
    }

    private static INotification CreateDomainEventNotification(IDomainEvent domainEvent)
    {
        var genericType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());

        return (INotification)Activator.CreateInstance(genericType, domainEvent.Id, domainEvent)!;
    }
}