namespace RestaurantManagement.Api.Features.MenuItems.GetMenuItems;

using FluentValidation;

public sealed class GetMenuItemsValidator : AbstractValidator<GetMenuItemsQuery>
{
    public GetMenuItemsValidator()
    {
        RuleFor(x => x.Category).MaximumLength(50).WithMessage("Category cannot exceed 50 characters");
    }
}