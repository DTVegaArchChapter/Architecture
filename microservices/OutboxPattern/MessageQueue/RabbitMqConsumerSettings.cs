namespace MessageQueue;

public sealed class RabbitMqConsumerSettings
{
    private List<string>? _exchanges;

    public List<string> Exchanges
    {
        get => _exchanges ??= new List<string>();
        set => _exchanges = value;
    }

    public string? Queue { get; set; }

    public bool SingleActiveConsumer { get; set; }
}