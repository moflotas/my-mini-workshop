using BuildingBlocks.Application.Contracts;
using MediatR;
using Quartz;

namespace BuildingBlocks.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public class ProcessOutboxJob(IMediator mediator) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await mediator.Send(new ProcessOutboxCommand());
    }
}