
using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.UpdateGoal;
internal sealed class UpdateGoalCommandHandler(IRepository<GoalSet> goalSetRepository) : ICommandHandler<UpdateGoalCommand, Result>
{
  public async Task<Result> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
  {
    var goalSet = await goalSetRepository.SingleOrDefaultAsync(new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken).ConfigureAwait(false);

    if (goalSet == null)
    {
      return Result.Error($"Goal set not found for id: {request.GoalSetId}");
    }

    var updateGoalResult = goalSet.UpdateGoal(request.GoalId, request.Title, request.GoalType, request.GoalValue);

    if (!updateGoalResult.IsSuccess)
    {
      return updateGoalResult.ToResult();
    }

    await goalSetRepository.UpdateAsync(goalSet, cancellationToken).ConfigureAwait(false);

    return Result.Success();
  }
}
