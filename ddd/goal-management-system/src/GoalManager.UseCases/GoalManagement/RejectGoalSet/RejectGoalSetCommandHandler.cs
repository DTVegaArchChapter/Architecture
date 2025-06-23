using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.RejectGoalSet;

internal sealed class RejectGoalSetCommandHandler(IRepository<GoalSet> goalSetRepository) : ICommandHandler<RejectGoalSetCommand, Result>
{
  public async Task<Result> Handle(RejectGoalSetCommand request, CancellationToken cancellationToken)
  {
    var goalSet = await goalSetRepository.SingleOrDefaultAsync(new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken);

    if (goalSet == null)
    {
      return Result.Error($"GoalSet not found: {request.GoalSetId}");
    }

    var result = goalSet.Reject();
    if (!result.IsSuccess)
    {
      return result;
    }

    await goalSetRepository.UpdateAsync(goalSet, cancellationToken).ConfigureAwait(false);

    return Result.SuccessWithMessage("Goal set is rejected");
  }
}
