using Microsoft.EntityFrameworkCore.Storage;
using RestaurantManagement.Domain.Repositories;
using RestaurantManagement.Infrastructure.Data;

namespace RestaurantManagement.Infrastructure.Repositories;

public sealed class UnitOfWork(
    RestaurantDbContext context,
    ITableRepository tableRepository,
    IMenuItemRepository menuItemRepository,
    IOrderRepository orderRepository) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public ITableRepository Tables => tableRepository;
    public IMenuItemRepository MenuItems => menuItemRepository;
    public IOrderRepository Orders => orderRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        context.Dispose();
    }
}