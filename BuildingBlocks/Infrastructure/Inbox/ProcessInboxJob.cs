using MediatR;
using Quartz;

namespace BuildingBlocks.Infrastructure.Inbox;

[DisallowConcurrentExecution]
public class ProcessInboxJob(IMediator mediator) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await mediator.Send(new ProcessInboxCommand());
    }
}