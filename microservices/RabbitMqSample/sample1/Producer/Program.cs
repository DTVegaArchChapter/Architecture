using System.Text;

using RabbitMQ.Client;

Console.WriteLine("Press [enter] to start.");
Console.ReadLine();

var factory = new ConnectionFactory { HostName = "localhost" };
await using (var connection = await factory.CreateConnectionAsync().ConfigureAwait(false))
await using (var channel = await connection.CreateChannelAsync().ConfigureAwait(false))
{
    // Direct exchange sample
    await channel.QueueDeclareAsync(queue: "sample1",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null).ConfigureAwait(false);

    for (var i = 0; i < 1000; i++)
    {
        var message = $"Message {i}";
        var body = Encoding.UTF8.GetBytes(message);
        await channel.BasicPublishAsync(exchange: "",
            routingKey: "sample1",
            body: body).ConfigureAwait(false);
        Console.WriteLine("'{0}' is sent", i);

        Thread.Sleep(25);
    }
}

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();