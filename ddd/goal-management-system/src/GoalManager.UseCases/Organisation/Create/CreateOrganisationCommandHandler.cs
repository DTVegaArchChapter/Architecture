using GoalManager.Core.Organisation;

namespace GoalManager.UseCases.Organisation.Create;

internal sealed class CreateOrganisationCommandHandler(IOrganisationService organisationService) : ICommandHandler<CreateOrganisationCommand, Result<int>>
{
  public Task<Result<int>> Handle(CreateOrganisationCommand request, CancellationToken cancellationToken)
  {
    return organisationService.CreateOrganisation(request.Name, cancellationToken);
  }
}
