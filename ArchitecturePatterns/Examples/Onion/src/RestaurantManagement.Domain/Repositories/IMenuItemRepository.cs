using RestaurantManagement.Domain.Entities;

namespace RestaurantManagement.Domain.Repositories;

public interface IMenuItemRepository
{
    Task<MenuItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MenuItem>> GetAvailableAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MenuItem>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MenuItem>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task AddAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
}