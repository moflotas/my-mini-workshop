using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.Configuration.Processing;

public class UnitOfWorkCommandHandlerDecorator<T>(
    ICommandHandler<T> decorated,
    IUnitOfWork unitOfWork,
    IAppDbContext dbContext
) : ICommandHandler<T> where T : ICommand
{
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase)
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
    }
}