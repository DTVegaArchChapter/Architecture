using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.AddGoal;

internal sealed class AddGoalCommandHandler(IRepository<GoalSet> goalSetRepository) : ICommandHandler<AddGoalCommand, Result<(int TeamId, int PeriodId)>>
{
  public async Task<Result<(int TeamId, int PeriodId)>> Handle(AddGoalCommand request, CancellationToken cancellationToken)
  {
    var goalSet = await goalSetRepository.SingleOrDefaultAsync(new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken).ConfigureAwait(false);
    if (goalSet == null)
    {
      return Result.Error($"Goal set not found for id: {request.GoalSetId}");
    }

    var goalValueResult = GoalValue.Create(request.MinValue, request.MidValue, request.MaxValue, request.GoalValueType);
    if (!goalValueResult.IsSuccess)
    {
      return goalValueResult.ToResult();
    }

    var addGoalResult = goalSet.AddGoal(request.Title, request.GoalType, goalValueResult.Value, request.Percentage);
    if (!addGoalResult.IsSuccess)
    {
      return addGoalResult.ToResult();
    }



    await goalSetRepository.UpdateAsync(goalSet, cancellationToken).ConfigureAwait(false);


    return (goalSet.TeamId, goalSet.PeriodId);
  }
}
