using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.ApproveGoalSet;

internal sealed class ApproveGoalSetCommandHandler(IRepository<GoalSet> goalSetRepository) : ICommandHandler<ApproveGoalSetCommand, Result>
{
  public async Task<Result> Handle(ApproveGoalSetCommand request, CancellationToken cancellationToken)
  {
    var goalSet = await goalSetRepository.SingleOrDefaultAsync(new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken);

    if (goalSet == null)
    {
      return Result.Error($"GoalSet not found: {request.GoalSetId}");
    }

    var result = goalSet.Approve(request.UserId);
    if (!result.IsSuccess)
    {
      return result;
    }

    await goalSetRepository.UpdateAsync(goalSet, cancellationToken).ConfigureAwait(false);

    return Result.SuccessWithMessage("Goal set is approved");
  }
}
