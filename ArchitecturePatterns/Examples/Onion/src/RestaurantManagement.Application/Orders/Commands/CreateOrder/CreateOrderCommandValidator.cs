namespace RestaurantManagement.Application.Orders.Commands.CreateOrder;

using FluentValidation;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.TableId)
            .GreaterThan(0)
            .WithMessage("Table ID must be greater than 0");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one order item is required");

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemRequestValidator());

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters");
    }
}

public sealed class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
{
    public OrderItemRequestValidator()
    {
        RuleFor(x => x.MenuItemId)
            .GreaterThan(0)
            .WithMessage("Menu item ID must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.SpecialInstructions)
            .MaximumLength(250)
            .WithMessage("Special instructions cannot exceed 250 characters");
    }
}