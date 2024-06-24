using BuildingBlocks.Application.Contracts;
using BuildingBlocks.Infrastructure.Configuration.Processing;

namespace BuildingBlocks.Infrastructure.Inbox;

public class ProcessInboxCommand : CommandBase, IRecurringCommand;