using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Application.Contracts;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace BuildingBlocks.Infrastructure.Configuration.Processing;

public class LoggingCommandHandlerDecorator<T>(
    ILogger logger,
    IExecutionContextAccessor executionContextAccessor,
    ICommandHandler<T> decorated)
    : ICommandHandler<T>
    where T : ICommand
{
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        if (command is IRecurringCommand)
        {
            await decorated.Handle(command, cancellationToken);

            return;
        }

        using (
            LogContext.Push(
                new RequestLogEnricher(executionContextAccessor),
                new CommandLogEnricher(command)))
        {
            try
            {
                logger.Information(
                    "Executing command {Command}",
                    command.GetType().Name);

                await decorated.Handle(command, cancellationToken);

                logger.Information("Command {Command} processed successful", command.GetType().Name);
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Command {Command} processing failed", command.GetType().Name);
                throw;
            }
        }
    }

    private class CommandLogEnricher(ICommand command) : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{command.Id.ToString()}")));
        }
    }

    private class RequestLogEnricher(IExecutionContextAccessor executionContextAccessor) : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (executionContextAccessor.IsAvailable)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(executionContextAccessor.CorrelationId)));
            }
        }
    }
}