using GoalManager.Core.Organisation;

namespace GoalManager.UseCases.Organisation.Delete;

internal class DeleteOrganisationCommandHandler(IOrganisationService organisationService) : ICommandHandler<DeleteOrganisationCommand, Result>
{
  public Task<Result> Handle(DeleteOrganisationCommand request, CancellationToken cancellationToken)
  {
    return organisationService.DeleteOrganisation(request.Id, cancellationToken);
  }
}
