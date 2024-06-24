using System.Text.Json;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.InternalCommands;
using MediatR;
using Modules.Events.Application.Users.CreateUser;
using Modules.UserAccess.IntegrationEvents;

namespace Modules.Events.Application.Users;

public class UserCreatedIntegrationEventHandler(IAppDbContext dbContext)
    : INotificationHandler<UserCreatedIntegrationEvent>
{
    public async Task Handle(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var createUserCommand = new CreateUserCommand(
            notification.UserId,
            notification.Email,
            notification.FirstNameRu,
            notification.LastNameRu,
            notification.PatronymicRu,
            notification.FirstNameEn,
            notification.LastNameEn,
            notification.PatronymicEn
        );

        await dbContext.InternalCommands.AddAsync(
            new InternalCommand(
                notification.Id,
                DateTime.UtcNow,
                createUserCommand.GetType().FullName!,
                JsonSerializer.Serialize(notification)
            ),
            cancellationToken
        );
    }
}