using RestaurantManagement.Domain.Entities;

namespace RestaurantManagement.Domain.Repositories;

public interface ITableRepository
{
    Task<Table?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Table?> GetByTableNumberAsync(int tableNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Table>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Table>> GetAvailableTablesAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Table table, CancellationToken cancellationToken = default);
    Task UpdateAsync(Table table, CancellationToken cancellationToken = default);
    Task DeleteAsync(Table table, CancellationToken cancellationToken = default);
}