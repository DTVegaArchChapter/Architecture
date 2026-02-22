using Mediator;
using RestaurantManagement.Application.Common;
using RestaurantManagement.Application.Common.DTOs;
using RestaurantManagement.Domain.Repositories;

namespace RestaurantManagement.Application.Tables.Queries.GetAllTables;

public sealed record GetAllTablesQuery : IRequest<Result<List<TableDto>>>;

public sealed class GetAllTablesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllTablesQuery, Result<List<TableDto>>>
{
    public async ValueTask<Result<List<TableDto>>> Handle(GetAllTablesQuery request, CancellationToken cancellationToken)
    {
        var tables = await unitOfWork.Tables.GetAllAsync(cancellationToken);
        
        var tableDtos = tables.Select(table => new TableDto(
            table.Id,
            table.TableNumber,
            table.Capacity,
            table.Status.ToString(),
            table.ReservedAt)).ToList();

        return Result<List<TableDto>>.Success(tableDtos);
    }
}