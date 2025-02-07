using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync().ConfigureAwait(false);
await using var channel = await connection.CreateChannelAsync().ConfigureAwait(false);
await channel.ExchangeDeclareAsync(exchange: "sample2", type: ExchangeType.Fanout).ConfigureAwait(false);

await channel.QueueDeclareAsync(queue: "sample2_queue1",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null).ConfigureAwait(false);
await channel.QueueBindAsync(queue: "sample2_queue1",
    exchange: "sample2",
    routingKey: "").ConfigureAwait(false);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (_, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine("Received '{0}'", message);

        return Task.CompletedTask;
    };
await channel.BasicConsumeAsync(queue: "sample2_queue1",
    autoAck: true,
    consumer: consumer).ConfigureAwait(false);

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();