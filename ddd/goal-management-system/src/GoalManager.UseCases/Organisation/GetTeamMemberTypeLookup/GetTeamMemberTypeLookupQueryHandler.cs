using GoalManager.Core.Organisation;

namespace GoalManager.UseCases.Organisation.GetTeamMemberTypeLookup;

internal sealed class GetTeamMemberTypeLookupQueryHandler : IQueryHandler<GetTeamMemberTypeLookupQuery, Result<List<TeamMemberTypeLookupDto>>>
{
  public Task<Result<List<TeamMemberTypeLookupDto>>> Handle(GetTeamMemberTypeLookupQuery request, CancellationToken cancellationToken)
  {
    return Task.FromResult(
      new Result<List<TeamMemberTypeLookupDto>>(
        TeamMemberType.List.Select(x => new TeamMemberTypeLookupDto(x.Value, x.Name)).ToList()));
  }
}
