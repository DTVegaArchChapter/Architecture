using Mediator;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Common;
using RestaurantManagement.Api.Contracts.Orders;
using RestaurantManagement.Application.Orders.Commands.CreateOrder;
using RestaurantManagement.Application.Orders.Commands.UpdateOrderStatus;
using RestaurantManagement.Application.Orders.Queries.GetKitchenOrders;
using RestaurantManagement.Domain.Entities;

namespace RestaurantManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var orderItems = request.Items.Select(item => new Application.Orders.Commands.CreateOrder.OrderItemRequest(
            item.MenuItemId, 
            item.Quantity, 
            item.SpecialInstructions)).ToList();

        var command = new CreateOrderCommand(request.TableId, orderItems, request.Notes);
        var result = await mediator.Send(command, cancellationToken);

        return result.ToApiResult(data => Results.Created($"/api/orders/{data?.Id}", data));
    }

    [HttpPut("{orderId:int}/status")]
    public async Task<IResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusRequest request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<OrderStatus>(request.NewStatus, true, out var status))
        {
            return Results.BadRequest("Invalid order status");
        }

        var command = new UpdateOrderStatusCommand(orderId, status);
        var result = await mediator.Send(command, cancellationToken);

        return result.ToApiResult();
    }

    [HttpGet("kitchen")]
    public async Task<IResult> GetKitchenOrders(CancellationToken cancellationToken)
    {
        var query = new GetKitchenOrdersQuery();
        var result = await mediator.Send(query, cancellationToken);

        return result.ToApiResult();
    }
}