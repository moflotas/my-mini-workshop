namespace BuildingBlocks.Infrastructure;

public interface IUnitOfWork
{
    Task<int> CommitAsync(Guid? internalCommandId = null, CancellationToken cancellationToken = default);
}