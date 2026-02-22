namespace RestaurantManagement.Application.Common.DTOs;

public record MenuItemDto(
    int Id,
    string Name,
    string Category,
    decimal Price,
    string? Description,
    bool IsAvailable);