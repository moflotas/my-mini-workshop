using Autofac;
using BuildingBlocks.Application.Contracts;
using Modules.UserAccess.Application.Contracts;

namespace Modules.UserAccess.Infrastructure;

public class UserAccessModule(IContainer container) : ModuleBase(container), IUserAccessModule;