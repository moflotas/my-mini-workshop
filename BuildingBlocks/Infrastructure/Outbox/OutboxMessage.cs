using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BuildingBlocks.Infrastructure.EventBus;

namespace BuildingBlocks.Infrastructure.Outbox;

public class OutboxMessage(Guid id, DateTime occurredOn, string type, string data, DateTime? processedDate = null)
{
    [Key]
    public Guid Id { get; set; } = id;

    public DateTime OccurredOn { get; set; } = occurredOn;

    public string Type { get; set; } = type;

    public string Data { get; set; } = data;

    public DateTime? ProcessedDate { get; set; } = processedDate;

    public static OutboxMessage CreateFrom<T>(T integrationEvent) where T : IntegrationEvent
    {
        return new OutboxMessage(
            integrationEvent.Id,
            integrationEvent.OccurredOn,
            integrationEvent.GetType().FullName!,
            JsonSerializer.Serialize(integrationEvent)
        );
    }
}