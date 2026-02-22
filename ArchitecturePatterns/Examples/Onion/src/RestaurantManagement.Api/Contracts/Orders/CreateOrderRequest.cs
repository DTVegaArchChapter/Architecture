namespace RestaurantManagement.Api.Contracts.Orders;

public record CreateOrderRequest(int TableId, List<OrderItemRequest> Items, string? Notes);