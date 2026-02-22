using Mediator;
using RestaurantManagement.Application.Common;
using RestaurantManagement.Application.Common.DTOs;
using RestaurantManagement.Domain.Repositories;

namespace RestaurantManagement.Application.MenuItems.Queries.GetMenuItems;

public sealed record GetMenuItemsQuery : IRequest<Result<List<MenuItemDto>>>;

public sealed class GetMenuItemsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMenuItemsQuery, Result<List<MenuItemDto>>>
{
    public async ValueTask<Result<List<MenuItemDto>>> Handle(GetMenuItemsQuery request, CancellationToken cancellationToken)
    {
        var menuItems = await unitOfWork.MenuItems.GetAvailableAsync(cancellationToken);
        
        var menuItemDtos = menuItems.Select(item => new MenuItemDto(
            item.Id,
            item.Name,
            item.Category,
            item.Price,
            item.Description,
            item.IsAvailable)).ToList();

        return Result<List<MenuItemDto>>.Success(menuItemDtos);
    }
}