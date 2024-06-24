namespace BuildingBlocks.Infrastructure.Inbox;

public class InboxMessage(Guid id, DateTime occurredOn, string type, string data, DateTime? processedDate = null)
{
    public Guid Id { get; set; } = id;

    public DateTime OccurredOn { get; set; } = occurredOn;

    public string Type { get; set; } = type;

    public string Data { get; set; } = data;

    public DateTime? ProcessedDate { get; set; } = processedDate;
}