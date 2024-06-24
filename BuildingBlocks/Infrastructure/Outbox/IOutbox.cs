namespace BuildingBlocks.Infrastructure.Outbox;

public interface IOutbox
{
    Task Add(OutboxMessage message);
}