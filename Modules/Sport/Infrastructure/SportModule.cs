using Autofac;
using BuildingBlocks.Application.Contracts;
using Modules.Sport.Application.Contracts;

namespace Modules.Sport.Infrastructure;

public class SportModule(IContainer container) : ModuleBase(container), ISportModule;