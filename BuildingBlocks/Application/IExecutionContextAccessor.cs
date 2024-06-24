namespace BuildingBlocks.Application;

public interface IExecutionContextAccessor
{
    public Guid? UserId { get; }
    public Guid CorrelationId { get; }
    bool IsAvailable { get; }
}