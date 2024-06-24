using BuildingBlocks.Application.Contracts;

namespace BuildingBlocks.Application.Configuration.Commands;

public abstract class InternalCommandBase(Guid id) : ICommand
{
    public Guid Id { get; } = id;
}

public abstract class InternalCommandBase<TResult>(Guid id) : ICommand<TResult>
{
    protected InternalCommandBase() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; } = id;
}