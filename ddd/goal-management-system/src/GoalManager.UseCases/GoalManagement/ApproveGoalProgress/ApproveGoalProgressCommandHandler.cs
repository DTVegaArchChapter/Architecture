using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.ApproveGoalProgress;

public sealed class ApproveGoalProgressCommandHandler(IRepository<GoalSet> goalSetRepository)
    : ICommandHandler<ApproveGoalProgressCommand, Result>
{
  public async Task<Result> Handle(ApproveGoalProgressCommand request, CancellationToken cancellationToken)
  {
    var goalSet = await goalSetRepository.SingleOrDefaultAsync(new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken);

    if (goalSet == null)
    {
      return Result.Error($"GoalSet not found: {request.GoalSetId}");
    }

    var result = goalSet.ApproveGoalProgress(request.GoalId);

    if (!result.IsSuccess)
    {
      return result.ToResult();
    }

    await goalSetRepository.UpdateAsync(goalSet, cancellationToken);

    return result;
  }
}
