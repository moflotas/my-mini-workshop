using BuildingBlocks.Application.Events;
using BuildingBlocks.Infrastructure.Outbox;
using MediatR;
using Modules.UserAccess.Domain.Users.Events;
using Modules.UserAccess.IntegrationEvents;

namespace Modules.UserAccess.Application.Users;

public class UserCreatedNotificationHandler(IOutbox outbox)
    : INotificationHandler<DomainEventNotification<UserCreatedDomainEvent>>
{
    public async Task Handle(DomainEventNotification<UserCreatedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        var integrationEvent = new UserCreatedIntegrationEvent(
            e.Id,
            e.OccurredOn,
            e.UserId,
            e.Email,
            e.FirstNameRu,
            e.LastNameRu,
            e.PatronymicRu,
            e.FirstNameEn,
            e.LastNameEn,
            e.PatronymicEn);
        await outbox.Add(OutboxMessage.CreateFrom(integrationEvent));
    }
}