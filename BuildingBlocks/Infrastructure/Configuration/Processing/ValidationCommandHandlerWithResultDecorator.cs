using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Application.Contracts;
using FluentValidation;

namespace BuildingBlocks.Infrastructure.Configuration.Processing;

public class ValidationCommandHandlerWithResultDecorator<T, TResult>(
    IList<IValidator<T>> validators,
    ICommandHandler<T, TResult> decorated)
    : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    public Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        var errors = validators
            .Select(v => v.Validate(command))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Count != 0)
        {
            throw new InvalidCommandException(errors.Select(x => x.ErrorMessage).ToList());
        }

        return decorated.Handle(command, cancellationToken);
    }
}