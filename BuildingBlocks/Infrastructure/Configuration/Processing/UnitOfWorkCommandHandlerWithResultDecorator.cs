using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.Configuration.Processing;

public class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult>(
    ICommandHandler<T, TResult> decorated,
    IUnitOfWork unitOfWork,
    IAppDbContext dbContext)
    : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        var result = await decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase<TResult>)
        {
            var internalCommand = await dbContext
                .InternalCommands
                .FirstOrDefaultAsync(
                    predicate: x => x.Id == command.Id,
                    cancellationToken: cancellationToken
                );

            if (internalCommand != null)
            {
                internalCommand.ProcessedDate = DateTime.UtcNow;
            }
        }

        await unitOfWork.CommitAsync(null, cancellationToken);

        return result;
    }
}