using RestaurantManagement.Domain.Common;

namespace RestaurantManagement.Domain.Entities;

public class MenuItem : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Category { get; private set; } = null!;
    public decimal Price { get; private set; }
    public string? Description { get; private set; }
    public bool IsAvailable { get; private set; }

    private MenuItem() { } // For EF Core

    public MenuItem(string name, string category, decimal price, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be null or empty", nameof(category));
        if (price <= 0)
            throw new ArgumentException("Price must be positive", nameof(price));

        Name = name;
        Category = category;
        Price = price;
        Description = description;
        IsAvailable = true;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price must be positive", nameof(newPrice));

        Price = newPrice;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    public void MakeAvailable()
    {
        IsAvailable = true;
    }

    public void MakeUnavailable()
    {
        IsAvailable = false;
    }
}