using GoalManager.Core.Organisation;

namespace GoalManager.UseCases.Organisation.AddTeam;

internal sealed class AddTeamCommandHandler(IOrganisationService organisationService) : ICommandHandler<AddTeamCommand, Result>
{
  public Task<Result> Handle(AddTeamCommand request, CancellationToken cancellationToken)
  {
    return organisationService.AddNewTeam(request.OrganisationId, request.TeamName, cancellationToken);
  }
}
