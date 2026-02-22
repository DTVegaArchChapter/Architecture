namespace RestaurantManagement.Domain.Entities;

public enum OrderStatus
{
    Pending = 0,
    InPreparation = 1,
    Ready = 2,
    Served = 3,
    Cancelled = 4
}