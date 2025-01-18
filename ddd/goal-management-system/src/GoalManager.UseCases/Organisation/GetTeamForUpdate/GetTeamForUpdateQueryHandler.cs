namespace GoalManager.UseCases.Organisation.GetTeamForUpdate;

public sealed class GetTeamForUpdateQueryHandler(IOrganisationQueryService organisationQueryService) : IQueryHandler<GetTeamForUpdateQuery, Result<TeamForUpdateDto>>
{
  public async Task<Result<TeamForUpdateDto>> Handle(GetTeamForUpdateQuery request, CancellationToken cancellationToken)
  {
    var team = await organisationQueryService.GetTeamForUpdate(request.Id).ConfigureAwait(false);
    if (team == null)
    {
      return Result<TeamForUpdateDto>.Error("Team not found");
    }

    return team;
  }
}
