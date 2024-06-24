using BuildingBlocks.Application.Contracts;
using BuildingBlocks.Infrastructure.Configuration.Processing;

namespace BuildingBlocks.Infrastructure.Outbox;

public class ProcessOutboxCommand : CommandBase, IRecurringCommand;