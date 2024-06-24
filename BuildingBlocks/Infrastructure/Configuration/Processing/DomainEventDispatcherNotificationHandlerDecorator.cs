using BuildingBlocks.Infrastructure.DomainEventDispatching;
using MediatR;

namespace BuildingBlocks.Infrastructure.Configuration.Processing;

public class DomainEventDispatcherNotificationHandlerDecorator<T>(
    IDomainEventDispatcher domainEventsDispatcher,
    INotificationHandler<T> decorated)
    : INotificationHandler<T>
    where T : INotification
{
    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        await decorated.Handle(notification, cancellationToken);

        await domainEventsDispatcher.DispatchEventsAsync();
    }
}