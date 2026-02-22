using RestaurantManagement.Domain.Common;

namespace RestaurantManagement.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; private set; }
    public int MenuItemId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public string? SpecialInstructions { get; private set; }

    // Navigation properties
    public Order Order { get; private set; } = null!;
    public MenuItem MenuItem { get; private set; } = null!;

    private OrderItem() { } // For EF Core

    public OrderItem(int menuItemId, int quantity, decimal price, string? specialInstructions = null)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (price <= 0)
            throw new ArgumentException("Price must be positive", nameof(price));

        MenuItemId = menuItemId;
        Quantity = quantity;
        Price = price;
        SpecialInstructions = specialInstructions;
    }

    public decimal GetTotalPrice() => Price * Quantity;

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(newQuantity));

        Quantity = newQuantity;
    }

    public void UpdateSpecialInstructions(string? instructions)
    {
        SpecialInstructions = instructions;
    }
}