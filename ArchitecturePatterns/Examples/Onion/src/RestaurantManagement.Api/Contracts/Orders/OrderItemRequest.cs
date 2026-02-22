namespace RestaurantManagement.Api.Contracts.Orders;

public record OrderItemRequest(int MenuItemId, int Quantity, string? SpecialInstructions);