using Autofac;

namespace BuildingBlocks.Infrastructure;

public class ServiceProviderWrapper(ILifetimeScope lifeTimeScope) : IServiceProvider
{
    public object? GetService(Type serviceType) => lifeTimeScope.ResolveOptional(serviceType);
}