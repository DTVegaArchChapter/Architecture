using GoalManager.Core.GoalManagement.Specifications;
using GoalManager.Core.GoalManagement;
using GoalManager.Core;

namespace GoalManager.UseCases.GoalManagement.UpdateGoalProgress;
internal sealed class UpdateGoalProgressCommandHandler(IRepository<GoalSet> goalSetRepository) 
    : ICommandHandler<UpdateGoalProgressCommand, Result<(int GoalSetId, int GoalId)>>
{
  public async Task<Result<(int GoalSetId, int GoalId)>> Handle(UpdateGoalProgressCommand request, CancellationToken cancellationToken)
  {
    var goalSet = await goalSetRepository.SingleOrDefaultAsync(new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken);

    if (goalSet == null)
    {
      return Result.Error($"GoalSet not found: {request.GoalSetId}");
    }

    var result = goalSet.UpdateGoalProgress(
        request.GoalId,
        request.ActualValue,
        request.Comment,
        GoalProgressStatus.WaitingForApproval);

    if (!result.IsSuccess)
    {
      return result.ToResult();
    }

    await goalSetRepository.UpdateAsync(goalSet, cancellationToken);

    return Result.Success<(int GoalSetId, int GoalId)>((goalSet.Id, request.GoalId), "Goal Progress is waiting for approval");
  }
}
