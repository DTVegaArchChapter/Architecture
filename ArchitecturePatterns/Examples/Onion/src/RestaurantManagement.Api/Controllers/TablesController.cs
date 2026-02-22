using Mediator;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Common;
using RestaurantManagement.Api.Contracts.Tables;
using RestaurantManagement.Application.Tables.Commands.UpdateTableStatus;
using RestaurantManagement.Application.Tables.Queries.GetAllTables;
using RestaurantManagement.Domain.Entities;

namespace RestaurantManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TablesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetAllTables(CancellationToken cancellationToken)
    {
        var query = new GetAllTablesQuery();
        var result = await mediator.Send(query, cancellationToken);

        return result.ToApiResult();
    }

    [HttpPut("{tableId:int}/status")]
    public async Task<IResult> UpdateTableStatus(int tableId, [FromBody] UpdateTableStatusRequest request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<TableStatus>(request.NewStatus, true, out var status))
        {
            return Results.BadRequest("Invalid table status");
        }

        var command = new UpdateTableStatusCommand(tableId, status);
        var result = await mediator.Send(command, cancellationToken);

        return result.ToApiResult();
    }
}