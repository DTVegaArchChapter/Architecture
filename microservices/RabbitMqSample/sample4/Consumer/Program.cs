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

// single active consumer sample

await channel.QueueDeclareAsync(
    queue: "sample4",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: new Dictionary<string, object?> { { "x-single-active-consumer", true } }).ConfigureAwait(false);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (_, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine("Received '{0}'", message);

        return Task.Delay(sleepMs);
    };
await channel.BasicConsumeAsync(queue: "sample4", autoAck: true, consumer: consumer).ConfigureAwait(false);

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();