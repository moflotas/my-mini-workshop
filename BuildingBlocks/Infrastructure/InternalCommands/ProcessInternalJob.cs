using BuildingBlocks.Infrastructure.Inbox;
using MediatR;
using Quartz;

namespace BuildingBlocks.Infrastructure.InternalCommands;

[DisallowConcurrentExecution]
public class ProcessInternalJob(IMediator mediator) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await mediator.Send(new ProcessInternalCommand());
    }
}