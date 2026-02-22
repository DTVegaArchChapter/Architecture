using Mediator;
using RestaurantManagement.Application.Common;
using RestaurantManagement.Application.Common.DTOs;
using RestaurantManagement.Domain.Repositories;

namespace RestaurantManagement.Application.Orders.Queries.GetKitchenOrders;

public sealed record GetKitchenOrdersQuery : IRequest<Result<List<OrderDto>>>;

public sealed class GetKitchenOrdersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetKitchenOrdersQuery, Result<List<OrderDto>>>
{
    public async ValueTask<Result<List<OrderDto>>> Handle(GetKitchenOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await unitOfWork.Orders.GetKitchenOrdersAsync(cancellationToken);
        
        if (!orders.Any())
        {
            return Result<List<OrderDto>>.Success([]);
        }

        // Get all menu items for the orders
        var menuItemIds = orders.SelectMany(o => o.OrderItems.Select(oi => oi.MenuItemId)).Distinct();
        var menuItems = await unitOfWork.MenuItems.GetByIdsAsync(menuItemIds, cancellationToken);

        var orderDtos = orders.Select(order =>
        {
            var orderItemDtos = order.OrderItems.Select(oi =>
            {
                var menuItem = menuItems.First(m => m.Id == oi.MenuItemId);
                return new OrderItemDto(
                    oi.Id,
                    menuItem.Name,
                    oi.Quantity,
                    oi.Price,
                    oi.SpecialInstructions);
            }).ToList();

            return new OrderDto(
                order.Id,
                order.OrderNumber,
                order.TableId,
                order.OrderDate,
                order.Status.ToString(),
                order.TotalAmount,
                order.Notes,
                orderItemDtos);
        }).ToList();

        return Result<List<OrderDto>>.Success(orderDtos);
    }
}