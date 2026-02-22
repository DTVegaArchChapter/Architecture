namespace RestaurantManagement.Application.Common.DTOs;

public record OrderItemDto(
    int Id,
    string MenuItemName,
    int Quantity,
    decimal Price,
    string? SpecialInstructions);