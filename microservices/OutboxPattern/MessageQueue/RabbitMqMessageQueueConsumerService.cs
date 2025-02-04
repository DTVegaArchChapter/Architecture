namespace MessageQueue;

using System.Text;
using System.Text.Json;
using MessageQueue.Events;
using Polly;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public interface IMessageQueueConsumerService<out TEvent> : IDisposable
    where TEvent : EventBase
{
    void ConsumeMessage(ConsumeMessageAction<TEvent> consumeAction);
}

public interface IMessageQueueConsumerService : IDisposable
{
    void ConsumeMessage(ConsumeMessageAction consumeAction);
}

public delegate Task<bool> ConsumeMessageAction<in TEvent>(TEvent @event, string eventType);

public delegate Task<bool> ConsumeMessageAction(string body, string eventType);

public class RabbitMqGenericMessageQueueConsumerService : IMessageQueueConsumerService
{
    private readonly IRabbitMqConnection _connection;

    private readonly string _queue;

    private bool _disposed;

    private IChannel? _channel;

    private string? _consumerTag;

    private readonly bool _singleActiveConsumer;

    private readonly IList<string> _exchanges;

    public RabbitMqGenericMessageQueueConsumerService(IRabbitMqConnection connection, RabbitMqConsumerSettings settings)
    {
        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        if (string.IsNullOrWhiteSpace(settings.Queue))
        {
            throw new ArgumentException($"Settings.{nameof(settings.Queue)} Value cannot be null or whitespace.", nameof(settings));
        }

        if (settings.Exchanges == null || settings.Exchanges.Count == 0)
        {
            throw new ArgumentException($"Settings.{nameof(settings.Exchanges)} Value cannot be null or empty.", nameof(settings));
        }

        _connection = connection ?? throw new ArgumentNullException(nameof(connection));

        _queue = settings.Queue;
        _exchanges = settings.Exchanges;
        _singleActiveConsumer = settings.SingleActiveConsumer;
    }

    public void ConsumeMessage(ConsumeMessageAction consumeAction)
    {
        if (consumeAction == null)
        {
            throw new ArgumentNullException(nameof(consumeAction));
        }

        Policy
            .Handle<Exception>()
            .WaitAndRetry(10, r => TimeSpan.FromSeconds(5))
            .Execute(
               async () =>
               {
                   _channel = await _connection.CreateChannel().ConfigureAwait(false);

                   foreach (var exchange in _exchanges)
                   {
                       await _channel.ExchangeDeclareAsync(exchange, "fanout", true, false, null).ConfigureAwait(false);
                   }

                   await _channel.QueueDeclareAsync(
                       _queue,
                       true,
                       false,
                       false,
                       arguments: new Dictionary<string, object>
                                      {
                                               {"x-single-active-consumer", _singleActiveConsumer}
                                      }!).ConfigureAwait(false);

                   foreach (var exchange in _exchanges)
                   {
                       await _channel.QueueBindAsync(_queue, exchange, string.Empty).ConfigureAwait(false);
                   }

                   var consumer = new AsyncEventingBasicConsumer(_channel);
                   consumer.ReceivedAsync += async (_, e) =>
                   {
                       try
                       {
                           var ack = await consumeAction(Encoding.UTF8.GetString(e.Body.Span), GetEventName(e)).ConfigureAwait(false);

                           if (ack)
                           {
                               await _channel.BasicAckAsync(e.DeliveryTag, false).ConfigureAwait(false);
                           }
                           else
                           {
                               await _channel.BasicNackAsync(e.DeliveryTag, false, true).ConfigureAwait(false);
                           }
                       }
                       catch (Exception)
                       {
                           await _channel.BasicNackAsync(e.DeliveryTag, false, true).ConfigureAwait(false);

                           throw;
                       }
                   };
                   await _channel.BasicQosAsync(0, 1, false).ConfigureAwait(false);
                   _consumerTag = await _channel.BasicConsumeAsync(_queue, false, consumer).ConfigureAwait(false);
               });
    }

    private static string GetEventName(BasicDeliverEventArgs e)
    {
        string eventName;
        if (e.BasicProperties.Headers != null && e.BasicProperties.Headers.TryGetValue("EventName", out var eventNameObject)
            && eventNameObject is byte[] eventNameBytes)
        {
            eventName = Encoding.UTF8.GetString(eventNameBytes);
        }
        else
        {
            eventName = "None";
        }

        return eventName;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
            }

            if (_consumerTag != null)
            {
                _channel?.BasicCancelAsync(_consumerTag).GetAwaiter().GetResult();
            }

            _channel?.CloseAsync(200, "Goodbye").GetAwaiter().GetResult();
            _channel?.Dispose();
            _disposed = true;
        }
    }

    ~RabbitMqGenericMessageQueueConsumerService()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

public class RabbitMqMessageQueueConsumerService<TEvent> : RabbitMqGenericMessageQueueConsumerService, IMessageQueueConsumerService<TEvent>
    where TEvent : EventBase
{
    public RabbitMqMessageQueueConsumerService(IRabbitMqConnection connection, RabbitMqConsumerSettings settings)
        : base(connection, SetExchangeName(settings))
    {
    }

    public void ConsumeMessage(ConsumeMessageAction<TEvent> consumeAction)
    {
        if (consumeAction == null)
        {
            throw new ArgumentNullException(nameof(consumeAction));
        }

        ConsumeMessage(
            async (body, eventType) =>
            {
                var @event = JsonSerializer.Deserialize<TEvent>(body);
                return @event != null && await consumeAction(@event, eventType).ConfigureAwait(false);
            });
    }

    private static RabbitMqConsumerSettings SetExchangeName(RabbitMqConsumerSettings settings)
    {
        settings.Exchanges = [EventNameAttribute.GetEventName(typeof(TEvent))];
        return settings;
    }
}