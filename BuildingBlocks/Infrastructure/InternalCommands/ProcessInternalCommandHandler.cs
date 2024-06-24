using System.Text.Json;
using BuildingBlocks.Application.Configuration.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BuildingBlocks.Infrastructure.InternalCommands;

public class ProcessInternalCommandHandler(IMediator mediator, IAppDbContext dbContext, ILogger logger)
    : ICommandHandler<ProcessInternalCommand>
{
    public async Task Handle(ProcessInternalCommand command, CancellationToken cancellationToken)
    {
        var internalCommands = await dbContext.InternalCommands
            .Where(im => im.ProcessedDate == null)
            .OrderBy(im => im.OccurredOn)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var message in internalCommands)
        {
            var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => message.Type.Contains(assembly.GetName().Name!));
            var type = messageAssembly?.GetType(message.Type);

            message.ProcessedDate = DateTime.UtcNow;

            if (type is null)
            {
                logger.Error("Type {Type} not found", message.Type);
                continue;
            }

            var request = JsonSerializer.Deserialize(message.Data, type);

            if (request is null)
            {
                logger.Error("Request {Request} could not be deserialized", message.Data);
                continue;
            }


            try
            {
                await mediator.Send(request, cancellationToken);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error while processing message {Message}", message);
                throw;
            }
        }
    }
}