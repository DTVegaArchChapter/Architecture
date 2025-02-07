using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var sleepMs = 0;
if (args.Length > 0)
{
    _ = int.TryParse(args[0], out sleepMs);
}

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync().ConfigureAwait(false);
await using var channel = await connection.CreateChannelAsync().ConfigureAwait(false);

// ack / nack sample

await channel.QueueDeclareAsync(queue: "sample3",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null).ConfigureAwait(false);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (_, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine("Received '{0}'", message);

        await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false).ConfigureAwait(false);
        // await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true).ConfigureAwait(false);

        Thread.Sleep(sleepMs);
    };
await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false).ConfigureAwait(false);
await channel.BasicConsumeAsync(queue: "sample3", autoAck: false, consumer: consumer).ConfigureAwait(false);

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();