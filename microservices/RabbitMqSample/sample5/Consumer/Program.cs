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

// dead letter exchange sample

await channel.ExchangeDeclareAsync(exchange: "sample5-dlx", type: ExchangeType.Direct).ConfigureAwait(false);
await channel.QueueDeclareAsync(
    queue: "sample5-dlx",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null).ConfigureAwait(false);
await channel.QueueBindAsync("sample5-dlx", "sample5-dlx", "sample5-dlx").ConfigureAwait(false);

await channel.QueueDeclareAsync(
    queue: "sample5",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: new Dictionary<string, object?>
                   {
                       { "x-dead-letter-exchange", "sample5-dlx" },
                       { "x-dead-letter-routing-key", "sample5-dlx" }
                   }).ConfigureAwait(false);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (_, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine("Received '{0}'", message);
            
        await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false).ConfigureAwait(false);

        Thread.Sleep(sleepMs);
    };
await channel.BasicConsumeAsync(queue: "sample5", autoAck: false, consumer: consumer).ConfigureAwait(false);

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();