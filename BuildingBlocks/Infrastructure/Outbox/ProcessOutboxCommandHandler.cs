using System.Text.Json;
using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Infrastructure.EventBus;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace BuildingBlocks.Infrastructure.Outbox;

public class ProcessOutboxCommandHandler(IEventBus eventBus, IAppDbContext dbContext, ILogger logger)
    : ICommandHandler<ProcessOutboxCommand>
{
    public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        var outboxMessages = await dbContext.OutboxMessages
            .Where(im => im.ProcessedDate == null)
            .OrderBy(im => im.OccurredOn)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var message in outboxMessages)
        {
            var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => message.Type.Contains(assembly.GetName().Name!));
            var type = messageAssembly?.GetType(message.Type);

            if (type is null)
            {
                logger.Error("Type {Type} not found", message.Type);
                continue;
            }

            if (JsonSerializer.Deserialize(message.Data, type) is not IntegrationEvent request)
            {
                logger.Error("Request {Request} could not be deserialized", message.Data);
                continue;
            }

            using (LogContext.Push(new OutboxMessageContextEnricher(request)))
            {
                await eventBus.Publish(request);

                message.ProcessedDate = DateTime.UtcNow;
            }
        }
    }

    private class OutboxMessageContextEnricher(IntegrationEvent integrationEvent) : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("Context",
                new ScalarValue($"OutboxMessage:{integrationEvent.Id.ToString()}")));
        }
    }
}