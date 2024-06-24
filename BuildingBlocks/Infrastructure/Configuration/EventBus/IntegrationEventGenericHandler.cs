using System.Text.Json;
using Autofac;
using BuildingBlocks.Infrastructure.EventBus;
using BuildingBlocks.Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BuildingBlocks.Infrastructure.Configuration.EventBus;

internal class IntegrationEventGenericHandler<T>(Func<ILifetimeScope> createScope) : IIntegrationEventHandler<T>
    where T : IntegrationEvent
{
    public async Task Handle(T integrationEvent)
    {
        await using var scope = createScope();

        var logger = scope.Resolve<ILogger>();
        var dbContext = scope.Resolve<IAppDbContext>();

        if (await dbContext.InboxMessages.Where(im => im.Id == integrationEvent.Id).AnyAsync())
        {
            logger.Warning("Integration event with id " + integrationEvent.Id + " already received");
            return;
        }
        
        dbContext.InboxMessages.Add(
            new InboxMessage(
                integrationEvent.Id,
                integrationEvent.OccurredOn,
                integrationEvent.GetType().FullName!,
                JsonSerializer.Serialize(integrationEvent)
            )
        );

        await dbContext.SaveAsync();
    }

    public Task Handle(IntegrationEvent integrationEvent)
    {
        return Handle((T)integrationEvent);
    }
}