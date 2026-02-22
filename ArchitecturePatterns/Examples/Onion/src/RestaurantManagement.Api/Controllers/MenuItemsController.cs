using Mediator;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Common;
using RestaurantManagement.Application.MenuItems.Queries.GetMenuItems;

namespace RestaurantManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetMenuItems(CancellationToken cancellationToken)
    {
        var query = new GetMenuItemsQuery();
        var result = await mediator.Send(query, cancellationToken);

        return result.ToApiResult();
    }
}