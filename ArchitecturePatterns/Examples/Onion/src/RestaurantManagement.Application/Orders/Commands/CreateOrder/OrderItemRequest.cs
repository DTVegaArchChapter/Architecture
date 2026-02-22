namespace RestaurantManagement.Application.Orders.Commands.CreateOrder;

public record OrderItemRequest(int MenuItemId, int Quantity, string? SpecialInstructions);