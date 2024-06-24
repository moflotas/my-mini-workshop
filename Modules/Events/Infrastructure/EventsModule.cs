using Autofac;
using BuildingBlocks.Application.Contracts;
using Modules.Events.Application.Contracts;

namespace Modules.Events.Infrastructure;

public class EventsModule(IContainer container) : ModuleBase(container), IEventsModule;