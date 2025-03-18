
using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.UpdateGoal;
internal sealed class UpdateGoalCommandHandler(IRepository<GoalSet> goalSetRepository) : ICommandHandler<UpdateGoalCommand, Result<(int GoalSetId, int GoalId)>>
{
  public async Task<Result<(int GoalSetId, int GoalId)>> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
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

    var goalValueResult = GoalValue.Create(goal.GoalValue.MinValue, goal.GoalValue.MidValue, goal.GoalValue.MaxValue, request.GoalValueType);

    if (!goalValueResult.IsSuccess)
    {
      return goalValueResult.ToResult();
    }

    var updateGoalResult = goalSet.UpdateGoal(request.GoalId, request.Title, request.GoalType, goalValueResult.Value);

    if (!updateGoalResult.IsSuccess)
    {
      return updateGoalResult.ToResult();
    }

    await goalSetRepository.UpdateAsync(goalSet, cancellationToken).ConfigureAwait(false);

    return (goalSet.Id, goal.Id);
  }
}
