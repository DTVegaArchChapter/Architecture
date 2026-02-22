namespace RestaurantManagement.Application.Tables.Commands.UpdateTableStatus;

using FluentValidation;

public sealed class UpdateTableStatusCommandValidator : AbstractValidator<UpdateTableStatusCommand>
{
    public UpdateTableStatusCommandValidator()
    {
        RuleFor(x => x.TableId)
            .GreaterThan(0)
            .WithMessage("Table ID must be greater than 0");

        RuleFor(x => x.NewStatus)
            .IsInEnum()
            .WithMessage("Invalid table status");
    }
}