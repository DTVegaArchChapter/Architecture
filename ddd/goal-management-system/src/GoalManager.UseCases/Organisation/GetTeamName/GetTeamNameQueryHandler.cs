namespace GoalManager.UseCases.Organisation.GetTeamName;

internal sealed class GetTeamNameQueryHandler(IOrganisationQueryService organisationQueryService) : IQueryHandler<GetTeamNameQuery, Result<string>>
{
  public async Task<Result<string>> Handle(GetTeamNameQuery request, CancellationToken cancellationToken)
  {
    var teamName = await organisationQueryService.GetTeamNameAsync(request.teamId).ConfigureAwait(false);
    if (teamName == null)
    {
      return Result<string>.Error("Team not found");
    }

    return teamName;
  }
}
