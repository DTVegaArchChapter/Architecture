using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;
using GoalManager.UseCases.Identity;
using GoalManager.UseCases.Organisation;

namespace GoalManager.UseCases.GoalManagement.GetGoalSet;

public sealed class GetGoalSetQueryHandler(
  IGoalManagementQueryService goalManagementQueryService, 
  IOrganisationQueryService organisationQueryService,
  IIdentityQueryService identityQueryService,
  IRepository<GoalPeriod> goalPeriodRepository, 
  IRepository<GoalSet> goalSetRepository) : IQueryHandler<GetGoalSetQuery, Result<GoalSetDto>>
{
  public async Task<Result<GoalSetDto>> Handle(GetGoalSetQuery request, CancellationToken cancellationToken)
  {
    var goalSet = await goalManagementQueryService.GetGoalSet(request.TeamId, request.Year, request.UserId).ConfigureAwait(false);
    if (goalSet == null)
    {
      var period = await goalPeriodRepository.SingleOrDefaultAsync(new GoalPeriodByTeamIdAndYearSpec(request.TeamId, request.Year), cancellationToken);
      if (period == null)
      {
        return Result.Error($"{request.Year} year's goal period is not created for the specified team. Go to Update Team page and click Create Goal Period button.");
      }

      var newGoalSet = GoalSet.Create(request.TeamId, period.Id, request.UserId);
      await goalSetRepository.AddAsync(newGoalSet, cancellationToken).ConfigureAwait(false);
     
      goalSet = await goalManagementQueryService.GetGoalSet(request.TeamId, request.Year, request.UserId).ConfigureAwait(false);
    }

    if (goalSet == null)
    {
      return Result.Error("Goal set not found");
    }

    goalSet.TeamName = await organisationQueryService.GetTeamNameAsync(goalSet.TeamId).ConfigureAwait(false);
    goalSet.User = await identityQueryService.GetUserName(goalSet.UserId).ConfigureAwait(false);

    return goalSet;
  }
}
