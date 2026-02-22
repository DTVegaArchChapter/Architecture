using Mediator;
using RestaurantManagement.Application.Common;
using RestaurantManagement.Application.Common.DTOs;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Repositories;

namespace RestaurantManagement.Application.Tables.Commands.UpdateTableStatus;

public sealed record UpdateTableStatusCommand(int TableId, TableStatus NewStatus) 
    : IRequest<Result<TableDto>>;

public sealed class UpdateTableStatusCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateTableStatusCommand, Result<TableDto>>
{
    public async ValueTask<Result<TableDto>> Handle(UpdateTableStatusCommand request, CancellationToken cancellationToken)
    {
        var table = await unitOfWork.Tables.GetByIdAsync(request.TableId, cancellationToken);
        if (table == null)
        {
            return Result<TableDto>.NotFound($"Table {request.TableId} not found");
        }

        // Apply business logic based on the new status
        switch (request.NewStatus)
        {
            case TableStatus.Available:
                table.MakeAvailable();
                break;
            case TableStatus.Occupied:
                table.Occupy();
                break;
            case TableStatus.Reserved:
                table.Reserve(DateTime.UtcNow);
                break;
            case TableStatus.OutOfService:
                table.TakeOutOfService();
                break;
            default:
                return Result<TableDto>.Failure($"Cannot update table to status: {request.NewStatus}");
        }

        await unitOfWork.Tables.UpdateAsync(table, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var tableDto = new TableDto(
            table.Id,
            table.TableNumber,
            table.Capacity,
            table.Status.ToString(),
            table.ReservedAt);

        return Result<TableDto>.Success(tableDto);
    }
}