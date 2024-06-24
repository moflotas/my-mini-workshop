﻿using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Application.Contracts;
using FluentValidation;

namespace BuildingBlocks.Infrastructure.Configuration.Processing;

public class ValidationCommandHandlerDecorator<T>(
    IList<IValidator<T>> validators,
    ICommandHandler<T> decorated)
    : ICommandHandler<T>
    where T : ICommand
{
    public async Task Handle(T command, CancellationToken cancellationToken)
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

        await decorated.Handle(command, cancellationToken);
    }
}