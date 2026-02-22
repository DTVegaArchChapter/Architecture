namespace RestaurantManagement.Api.Features.Orders.CreateOrder;

using FluentValidation;

public sealed class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.TableId).GreaterThan(0).WithMessage("Table ID must be greater than 0");
        RuleFor(x => x.Items).NotEmpty().WithMessage("Order must have at least one item");
        RuleFor(x => x.Notes).MaximumLength(500).WithMessage("Order notes cannot exceed 500 characters");
        RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.MenuItemId).GreaterThan(0).WithMessage("Menu item ID must be greater than 0");
                item.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
                item.RuleFor(x => x.SpecialInstructions).MaximumLength(250).WithMessage("Special instructions cannot exceed 250 characters");
            });
    }
}