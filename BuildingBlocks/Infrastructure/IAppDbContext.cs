using BuildingBlocks.Infrastructure.Inbox;
using BuildingBlocks.Infrastructure.InternalCommands;
using BuildingBlocks.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure;

public interface IAppDbContext
{
    public DbSet<InternalCommand> InternalCommands { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public Task<int> SaveAsync(CancellationToken cancellationToken = default);
}