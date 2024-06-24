namespace BuildingBlocks.Infrastructure.Outbox;

public class OutboxAccessor(IAppDbContext dbContext) : IOutbox
{
    public async Task Add(OutboxMessage message)
    {
        await dbContext.OutboxMessages.AddAsync(message);
    }
}