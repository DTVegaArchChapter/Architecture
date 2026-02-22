namespace RestaurantManagement.Application.Common.DTOs;

public record OrderDto(
    int Id,
    string OrderNumber,
    int TableId,
    DateTime OrderDate,
    string Status,
    decimal TotalAmount,
    string? Notes,
    List<OrderItemDto> OrderItems);