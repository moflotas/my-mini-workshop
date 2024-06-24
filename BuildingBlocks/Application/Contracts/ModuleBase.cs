using System.ComponentModel;
using Autofac;
using MediatR;
using IContainer = Autofac.IContainer;

namespace BuildingBlocks.Application.Contracts;

public class ModuleBase(IContainer container) : IModule
{
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        await using var scope = container.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        return await mediator.Send(command);
    }

    public async Task ExecuteCommandAsync(ICommand command)
    {
        await using var scope = container.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        await mediator.Send(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        await using var scope = container.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        return await mediator.Send(query);
    }
}