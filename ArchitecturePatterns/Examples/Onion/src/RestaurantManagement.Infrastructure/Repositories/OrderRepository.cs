using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Repositories;
using RestaurantManagement.Infrastructure.Data;

namespace RestaurantManagement.Infrastructure.Repositories;

public sealed class OrderRepository(RestaurantDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Table)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Table)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Table)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetByTableIdAsync(int tableId, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Table)
            .Where(o => o.TableId == tableId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Table)
            .Where(o => o.Status == status)
            .OrderBy(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetKitchenOrdersAsync(CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Table)
            .Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.InPreparation)
            .OrderBy(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await context.Orders.AddAsync(order, cancellationToken);
    }

    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        context.Orders.Update(order);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Order order, CancellationToken cancellationToken = default)
    {
        context.Orders.Remove(order);
        return Task.CompletedTask;
    }
}