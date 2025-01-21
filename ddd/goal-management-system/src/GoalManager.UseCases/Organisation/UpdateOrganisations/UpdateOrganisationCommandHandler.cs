using GoalManager.Core.Organisation;

namespace GoalManager.UseCases.Organisation.UpdateOrganisations;

public sealed class UpdateOrganisationCommandHandler(IOrganisationService organisationService) : ICommandHandler<UpdateOrganisationCommand, Result>
{
  public Task<Result> Handle(UpdateOrganisationCommand request, CancellationToken cancellationToken)
  {
    return organisationService.UpdateOrganisation(request.Id, request.Name, cancellationToken);
  }
}
