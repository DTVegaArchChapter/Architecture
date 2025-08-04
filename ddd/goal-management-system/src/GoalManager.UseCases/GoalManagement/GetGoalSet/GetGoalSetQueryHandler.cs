using GoalManager.UseCases.Identity;
using GoalManager.UseCases.Organisation;

namespace GoalManager.UseCases.GoalManagement.GetGoalSet;

public sealed class GetGoalSetQueryHandler(
  IGoalManagementQueryService goalManagementQueryService, 
  IOrganisationQueryService organisationQueryService,
  IIdentityQueryService identityQueryService) : IQueryHandler<GetGoalSetQuery, Result<GoalSetDto?>>
{
  public async Task<Result<GoalSetDto?>> Handle(GetGoalSetQuery request, CancellationToken cancellationToken)
  {
    var goalSet = await goalManagementQueryService.GetGoalSet(request.TeamId, request.Year, request.UserId).ConfigureAwait(false);
    if (goalSet == null)
    {
      return Result.Success<GoalSetDto?>(null);
    }

    goalSet.TeamName = await organisationQueryService.GetTeamNameAsync(goalSet.TeamId).ConfigureAwait(false);
    goalSet.User = await identityQueryService.GetUserName(goalSet.UserId).ConfigureAwait(false);

    return goalSet;
  }
}
