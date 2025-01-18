using GoalManager.Core.Organisation;

namespace GoalManager.UseCases.Organisation.UpdateTeam;

internal sealed class UpdateTeamCommandHandler(IOrganisationService organisationService) : ICommandHandler<UpdateTeamCommand, Result>
{
  public Task<Result> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
  {
    return organisationService.UpdateTeam(request.OrganisationId, request.TeamId, request.TeamName, cancellationToken);
  }
}
