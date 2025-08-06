using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.RejectGoalProgress;

public sealed class RejectGoalProgressCommandHandler(IRepository<GoalSet> goalSetRepository)
    : ICommandHandler<RejectGoalProgressCommand, Result>
{
  public async Task<Result> Handle(RejectGoalProgressCommand request, CancellationToken cancellationToken)
  {
    var goalSet = await goalSetRepository.SingleOrDefaultAsync(new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken);

    if (goalSet == null)
    {
      return Result.Error($"GoalSet not found: {request.GoalSetId}");
    }

    var result = goalSet.RejectGoalProgress(request.GoalId);

    if (!result.IsSuccess)
    {
      return result.ToResult();
    }

    await goalSetRepository.UpdateAsync(goalSet, cancellationToken);

    return result;
  }
}
