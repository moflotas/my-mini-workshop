using Autofac;
using BuildingBlocks.Infrastructure.EventBus;

namespace BuildingBlocks.Infrastructure.Configuration.EventBus;

public class EventBusModule(IEventBus? eventsBus) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        if (eventsBus != null)
        {
            builder.RegisterInstance(eventsBus).SingleInstance();
        }
        else
        {
            builder.RegisterType<InMemoryEventBusClient>()
                .As<IEventBus>()
                .SingleInstance();
        }
    }
}