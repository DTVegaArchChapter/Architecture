using Mediator;
using RestaurantManagement.Application.Common;
using RestaurantManagement.Application.Common.DTOs;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Repositories;

namespace RestaurantManagement.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(int TableId, List<OrderItemRequest> Items, string? Notes) 
    : IRequest<Result<OrderDto>>;

public sealed class CreateOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
{
    public async ValueTask<Result<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Validate table exists and is available
        var table = await unitOfWork.Tables.GetByIdAsync(request.TableId, cancellationToken);
        if (table == null)
        {
            return Result<OrderDto>.NotFound($"Table {request.TableId} not found");
        }

        if (!table.IsAvailable)
        {
            return Result<OrderDto>.Failure($"Table {table.TableNumber} is not available for orders");
        }

        // Get menu items
        var menuItemIds = request.Items.Select(i => i.MenuItemId).ToList();
        var menuItems = await unitOfWork.MenuItems.GetByIdsAsync(menuItemIds, cancellationToken);
        var availableMenuItems = menuItems.Where(m => m.IsAvailable).ToList();

        // Check which menu items are not available
        var availableMenuItemIds = availableMenuItems.Select(m => m.Id).ToList();
        var unavailableMenuItemIds = menuItemIds.Where(id => !availableMenuItemIds.Contains(id)).ToList();

        if (unavailableMenuItemIds.Count != 0)
        {
            return Result<OrderDto>.Failure(
                $"The following menu items are not available: {string.Join(", ", unavailableMenuItemIds)}",
                errorDetails: new Dictionary<string, object> { ["UnavailableMenuItemIds"] = unavailableMenuItemIds }
            );
        }

        // Create order
        var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd-HHmmss}";
        var order = new Order(orderNumber, request.TableId, request.Notes);

        // Add order items
        foreach (var itemRequest in request.Items)
        {
            var menuItem = availableMenuItems.First(m => m.Id == itemRequest.MenuItemId);
            order.AddOrderItem(itemRequest.MenuItemId, itemRequest.Quantity, menuItem.Price, itemRequest.SpecialInstructions);
        }

        await unitOfWork.Orders.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Return DTO
        var orderItemDtos = order.OrderItems.Select(oi =>
        {
            var menuItem = availableMenuItems.First(m => m.Id == oi.MenuItemId);
            return new OrderItemDto(
                oi.Id,
                menuItem.Name,
                oi.Quantity,
                oi.Price,
                oi.SpecialInstructions);
        }).ToList();

        var orderDto = new OrderDto(
            order.Id,
            order.OrderNumber,
            order.TableId,
            order.OrderDate,
            order.Status.ToString(),
            order.TotalAmount,
            order.Notes,
            orderItemDtos);

        return Result<OrderDto>.Success(orderDto);
    }
}