namespace BuildingBlocks.Infrastructure.EventBus;

public sealed class InMemoryEventBus
{
    private InMemoryEventBus()
    {
        _handlersDictionary = new Dictionary<string, List<IIntegrationEventHandler>>();
    }

    public static InMemoryEventBus Instance { get; } = new();

    private readonly IDictionary<string, List<IIntegrationEventHandler>> _handlersDictionary;

    public void Subscribe(IIntegrationEventHandler handler, string eventType)
    {
        if (_handlersDictionary.TryGetValue(eventType, out var value))
        {
            value.Add(handler);
        }
        else
        {
            _handlersDictionary.Add(eventType, [handler]);
        }
    }

    public async Task Publish(IntegrationEvent integrationEvent)
    {
        var eventType = integrationEvent.GetType().FullName;

        if (eventType == null)
        {
            return;
        }

        var integrationEventHandlers = _handlersDictionary[eventType];

        foreach (var integrationEventHandler in integrationEventHandlers)
        {
            await integrationEventHandler.Handle(integrationEvent);
        }
    }
}