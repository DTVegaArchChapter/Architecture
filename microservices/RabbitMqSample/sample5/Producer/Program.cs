using System.Text;

using RabbitMQ.Client;

Console.WriteLine("Press [enter] to start.");
Console.ReadLine();

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync().ConfigureAwait(false);
await using var channel = await connection.CreateChannelAsync().ConfigureAwait(false);

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

for (var i = 0; i < 1000; i++)
{
    var message = $"Message {i}";
    var body = Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(exchange: "",
        routingKey: "sample5",
        body: body).ConfigureAwait(false);
    Console.WriteLine("'{0}' is sent", i);

    Thread.Sleep(25);
}

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();