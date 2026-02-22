namespace RestaurantManagement.Application.Orders.Commands.UpdateOrderStatus;

using FluentValidation;

public sealed class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0)
            .WithMessage("Order ID must be greater than 0");

        RuleFor(x => x.NewStatus)
            .IsInEnum()
            .WithMessage("Invalid order status");
    }
}