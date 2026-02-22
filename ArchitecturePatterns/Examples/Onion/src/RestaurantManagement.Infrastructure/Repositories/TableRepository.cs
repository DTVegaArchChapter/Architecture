using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Repositories;
using RestaurantManagement.Infrastructure.Data;

namespace RestaurantManagement.Infrastructure.Repositories;

public sealed class TableRepository(RestaurantDbContext context) : ITableRepository
{
    public async Task<Table?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Tables.FindAsync([id], cancellationToken);
    }

    public async Task<Table?> GetByTableNumberAsync(int tableNumber, CancellationToken cancellationToken = default)
    {
        return await context.Tables
            .FirstOrDefaultAsync(t => t.TableNumber == tableNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<Table>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Tables
            .OrderBy(t => t.TableNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Table>> GetAvailableTablesAsync(CancellationToken cancellationToken = default)
    {
        return await context.Tables
            .Where(t => t.Status == TableStatus.Available)
            .OrderBy(t => t.TableNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Table table, CancellationToken cancellationToken = default)
    {
        await context.Tables.AddAsync(table, cancellationToken);
    }

    public Task UpdateAsync(Table table, CancellationToken cancellationToken = default)
    {
        context.Tables.Update(table);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Table table, CancellationToken cancellationToken = default)
    {
        context.Tables.Remove(table);
        return Task.CompletedTask;
    }
}