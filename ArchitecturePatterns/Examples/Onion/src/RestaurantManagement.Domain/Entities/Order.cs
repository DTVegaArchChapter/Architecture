using RestaurantManagement.Domain.Common;

namespace RestaurantManagement.Domain.Entities;

public class Order : BaseEntity
{
    public string OrderNumber { get; private set; } = null!;
    public int TableId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string? Notes { get; private set; }

    // Navigation properties
    public Table Table { get; private set; } = null!;
    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order() { } // For EF Core

    public Order(string orderNumber, int tableId, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new ArgumentException("Order number cannot be null or empty", nameof(orderNumber));

        OrderNumber = orderNumber;
        TableId = tableId;
        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        Notes = notes;
        TotalAmount = 0;
    }

    public void AddOrderItem(int menuItemId, int quantity, decimal price, string? specialInstructions = null)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot add items to order with status: {Status}");

        var orderItem = new OrderItem(menuItemId, quantity, price, specialInstructions);
        _orderItems.Add(orderItem);
        RecalculateTotal();
    }

    public void RemoveOrderItem(int orderItemId)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot remove items from order with status: {Status}");

        var item = _orderItems.FirstOrDefault(oi => oi.Id == orderItemId);
        if (item != null)
        {
            _orderItems.Remove(item);
            RecalculateTotal();
        }
    }

    public void UpdateOrderItemQuantity(int orderItemId, int newQuantity)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot update items in order with status: {Status}");

        var item = _orderItems.FirstOrDefault(oi => oi.Id == orderItemId);
        if (item != null)
        {
            item.UpdateQuantity(newQuantity);
            RecalculateTotal();
        }
    }

    public void StartPreparation()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot start preparation for order with status: {Status}");

        if (!_orderItems.Any())
            throw new InvalidOperationException("Cannot start preparation for order with no items");

        Status = OrderStatus.InPreparation;
    }

    public void MarkAsReady()
    {
        if (Status != OrderStatus.InPreparation)
            throw new InvalidOperationException($"Cannot mark order as ready with status: {Status}");

        Status = OrderStatus.Ready;
    }

    public void Serve()
    {
        if (Status != OrderStatus.Ready)
            throw new InvalidOperationException($"Cannot serve order with status: {Status}");

        Status = OrderStatus.Served;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Served)
            throw new InvalidOperationException("Cannot cancel a served order");

        Status = OrderStatus.Cancelled;
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
    }

    private void RecalculateTotal()
    {
        TotalAmount = _orderItems.Sum(item => item.GetTotalPrice());
    }

    public bool CanBeModified => Status == OrderStatus.Pending;
}