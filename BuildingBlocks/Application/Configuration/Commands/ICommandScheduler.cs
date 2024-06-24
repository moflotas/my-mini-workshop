﻿using BuildingBlocks.Application.Contracts;

namespace BuildingBlocks.Application.Configuration.Commands;

public interface ICommandScheduler
{
    Task EnqueueAsync(ICommand command);

    Task EnqueueAsync<T>(ICommand<T> command);
}