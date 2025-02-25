using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.GetGoalSet;

public sealed class GetGoalSetQueryHandler(IRepository<GoalPeriod> goalPeriodRepository, IRepository<GoalSet> goalSetRepository) : IQueryHandler<GetGoalSetQuery, Result<GoalSet>>
{
  public async Task<Result<GoalSet>> Handle(GetGoalSetQuery request, CancellationToken cancellationToken)
  {
    var period = await goalPeriodRepository.SingleOrDefaultAsync(new GoalPeriodByTeamIdAndYearSpec(request.TeamId, request.Year), cancellationToken);
    if (period == null)
    {
      return Result.Error($"{request.Year} year's goal period is not created for the specified team.");
    }

    var goalSet = await goalSetRepository.SingleOrDefaultAsync(new GoalSetWithGoalsByPeriodIdTeamIdUserIdSpec(period.Id, request.TeamId, request.UserId), cancellationToken);
    if (goalSet == null)
    {
      goalSet = GoalSet.Create(request.TeamId, period.Id, request.UserId);
      goalSet = await goalSetRepository.AddAsync(goalSet, cancellationToken);
    }

    return goalSet;
  }
}
