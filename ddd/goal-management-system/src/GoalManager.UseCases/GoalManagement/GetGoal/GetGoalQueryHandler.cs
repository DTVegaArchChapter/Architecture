using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.GetGoal;
public sealed class GetGoalQueryHandler(IRepository<GoalSet> goalSetRepository) : IQueryHandler<GetGoalQuery, Result<Goal>>
{
  public async Task<Result<Goal>> Handle(GetGoalQuery request, CancellationToken cancellationToken)
  {
    var goalSet = await goalSetRepository.SingleOrDefaultAsync(new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken).ConfigureAwait(false);

    if (goalSet == null)
    {
      return Result.Error($"Goal set not found for id: {request.GoalSetId}");
    }

    var goal = goalSet.Goals.SingleOrDefault(x => x.Id == request.GoalId);

    if (goal is null)
    {
      return Result.Error($"Goal not found for id: {request.GoalId}");
    }

    return goal;
  }
}
