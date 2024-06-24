using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.Inbox;
using BuildingBlocks.Infrastructure.InternalCommands;
using BuildingBlocks.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Modules.Events.Domain.Users;

namespace Modules.Events.Infrastructure;

public class EventsContext(DbContextOptions options) : AppDbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder.Entity(entity.Name).ToTable(entity.Name.Split(".").Last(), nameof(Events));
        }
    }
}