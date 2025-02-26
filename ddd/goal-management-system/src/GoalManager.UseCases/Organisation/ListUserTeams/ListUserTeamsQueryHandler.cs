using GoalManager.UseCases.Organisation;

namespace GoalManager.UseCases.Organisation.ListUserTeams;

public sealed class ListUserTeamsQueryHandler(IOrganisationQueryService organisationQueryService) : IQueryHandler<ListUserTeamsQuery, List<UserTeamListItemDto>>
{
  public Task<List<UserTeamListItemDto>> Handle(ListUserTeamsQuery request, CancellationToken cancellationToken)
  {
    return organisationQueryService.ListUserTeams(request.userId);
  }
}
