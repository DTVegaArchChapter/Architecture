using RestaurantManagement.Domain.Common;

namespace RestaurantManagement.Domain.Entities;

public class Table : BaseEntity
{
    public int TableNumber { get; private set; }
    public int Capacity { get; private set; }
    public TableStatus Status { get; private set; }
    public DateTime? ReservedAt { get; private set; }

    private Table() { } // For EF Core

    public Table(int tableNumber, int capacity)
    {
        if (tableNumber <= 0)
            throw new ArgumentException("Table number must be positive", nameof(tableNumber));
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be positive", nameof(capacity));

        TableNumber = tableNumber;
        Capacity = capacity;
        Status = TableStatus.Available;
    }

    public bool IsAvailable => Status == TableStatus.Available;

    public void Reserve(DateTime reservationTime)
    {
        if (Status != TableStatus.Available)
            throw new InvalidOperationException($"Cannot reserve table {TableNumber}. Current status: {Status}");

        Status = TableStatus.Reserved;
        ReservedAt = reservationTime;
    }

    public void Occupy()
    {
        if (Status != TableStatus.Available && Status != TableStatus.Reserved)
            throw new InvalidOperationException($"Cannot occupy table {TableNumber}. Current status: {Status}");

        Status = TableStatus.Occupied;
        ReservedAt = null;
    }

    public void MakeAvailable()
    {
        Status = TableStatus.Available;
        ReservedAt = null;
    }

    public void TakeOutOfService()
    {
        if (Status == TableStatus.Occupied)
            throw new InvalidOperationException($"Cannot take occupied table {TableNumber} out of service");

        Status = TableStatus.OutOfService;
        ReservedAt = null;
    }
}