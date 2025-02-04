namespace MessageQueue;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

using MessageQueue.Events;

using RabbitMQ.Client;

public interface IMessageQueuePublisherService
{
    Task PublishMessage<TEvent>([DisallowNull] TEvent @event)
        where TEvent : EventBase;
}

public class RabbitMqMessageQueuePublisherService : IMessageQueuePublisherService
{
    private readonly IRabbitMqConnection _connection;

    public RabbitMqMessageQueuePublisherService(IRabbitMqConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task PublishMessage<TEvent>([DisallowNull] TEvent @event)
        where TEvent : EventBase
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        await using var channel = await _connection.CreateChannel().ConfigureAwait(false);

        var eventType = @event.GetType();
        var eventName = EventNameAttribute.GetEventName(eventType);
        await channel.ExchangeDeclareAsync(eventName, "fanout", true, false, null).ConfigureAwait(false);

        var properties = new BasicProperties
                             {
                                 Headers = new Dictionary<string, object> { { "EventName", eventName } }!
                             };

        await channel.BasicPublishAsync(
            eventName,
            string.Empty,
            true,
            properties,
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event, eventType)));
    }
}