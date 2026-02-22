namespace RestaurantManagement.Api.Features.Tables.UpdateTableStatus;

using FluentValidation;

public sealed class UpdateTableStatusValidator : AbstractValidator<UpdateTableStatusCommand>
{
    public UpdateTableStatusValidator()
    {
        RuleFor(x => x.TableId).GreaterThan(0).WithMessage("Table ID must be greater than 0");
        RuleFor(x => x.NewStatus).IsInEnum().WithMessage("Invalid table status");
    }
}