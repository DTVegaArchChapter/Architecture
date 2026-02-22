using Mediator;
using RestaurantManagement.Application.Common;
using RestaurantManagement.Application.Common.DTOs;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Repositories;

namespace RestaurantManagement.Application.Orders.Commands.UpdateOrderStatus;

public sealed record UpdateOrderStatusCommand(int OrderId, OrderStatus NewStatus) 
    : IRequest<Result<OrderDto>>;

public sealed class UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrderStatusCommand, Result<OrderDto>>
{
    public async ValueTask<Result<OrderDto>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
        {
            return Result<OrderDto>.NotFound($"Order {request.OrderId} not found");
        }

        // Apply business logic based on the new status
        switch (request.NewStatus)
        {
            case OrderStatus.InPreparation:
                order.StartPreparation();
                break;
            case OrderStatus.Ready:
                order.MarkAsReady();
                break;
            case OrderStatus.Served:
                order.Serve();
                break;
            case OrderStatus.Cancelled:
                order.Cancel();
                break;
            default:
                return Result<OrderDto>.Failure($"Cannot update order to status: {request.NewStatus}");
        }

        await unitOfWork.Orders.UpdateAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Get menu items for DTO
        var menuItems = await unitOfWork.MenuItems.GetByIdsAsync(
                            order.OrderItems.Select(oi => oi.MenuItemId),
                            cancellationToken);

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