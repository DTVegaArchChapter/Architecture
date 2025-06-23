using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.SendGoalSetToApproval;

internal sealed class SendGoalSetToApprovalCommandHandler(IRepository<GoalSet> goalSetRepository) : ICommandHandler<SendGoalSetToApprovalCommand, Result>
{
  public async Task<Result> Handle(SendGoalSetToApprovalCommand request, CancellationToken cancellationToken)
  {
    var goalSet = await goalSetRepository.SingleOrDefaultAsync(new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken);

    if (goalSet == null)
    {
      return Result.Error($"GoalSet not found: {request.GoalSetId}");
    }

    var result = goalSet.SendToApproval();
    if (!result.IsSuccess)
    {
      return result;
    }

    await goalSetRepository.UpdateAsync(goalSet, cancellationToken);

    return Result.Success();
  }
}
