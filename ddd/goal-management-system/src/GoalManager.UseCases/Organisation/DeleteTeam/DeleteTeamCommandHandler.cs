using GoalManager.Core.Organisation;

namespace GoalManager.UseCases.Organisation.DeleteTeam;

internal sealed class DeleteTeamCommandHandler(IOrganisationService organisationService) : ICommandHandler<DeleteTeamCommand, Result>
{
  public Task<Result> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
  {
    return organisationService.DeleteTeam(request.OrganisationId, request.TeamId, cancellationToken);
  }
}
