namespace RestaurantManagement.Application.Common.DTOs;

public record TableDto(
    int Id,
    int TableNumber,
    int Capacity,
    string Status,
    DateTime? ReservedAt);