namespace TransactionalOutboxPatternApp.Infrastructure.Events;

using MessageQueue.Events;

[EventName("Order.OrderCreated")]
public sealed class OrderCreatedEvent : EventBase
{
    public int OrderId { get; }

    public OrderCreatedEvent(int orderId)
    {
        OrderId = orderId;
    }
}