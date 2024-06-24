using BuildingBlocks.Infrastructure.DomainEventDispatching;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure;

public class UnitOfWork(
    DbContext context,
    IDomainEventDispatcher domainEventDispatcher)
    : IUnitOfWork
{
    public async Task<int> CommitAsync(Guid? internalCommandId = null, CancellationToken cancellationToken = default)
    {
        await domainEventDispatcher.DispatchEventsAsync();

        return await context.SaveChangesAsync(cancellationToken);
    }
}