using System.Text;

using RabbitMQ.Client;

Console.WriteLine("Press [enter] to start.");
Console.ReadLine();

var factory = new ConnectionFactory { HostName = "localhost" };
await using (var connection = await factory.CreateConnectionAsync().ConfigureAwait(false))
await using (var channel = await connection.CreateChannelAsync().ConfigureAwait(false))
{
    // fanout exchange sample

    await channel.ExchangeDeclareAsync(exchange: "sample2", type: ExchangeType.Fanout).ConfigureAwait(false);

    var message = $"Message at {DateTime.Now}";
    var body = Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(exchange: "sample2",
        routingKey: "",
        body: body).ConfigureAwait(false);
    Console.WriteLine("'{0}' is sent", message);
}

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();