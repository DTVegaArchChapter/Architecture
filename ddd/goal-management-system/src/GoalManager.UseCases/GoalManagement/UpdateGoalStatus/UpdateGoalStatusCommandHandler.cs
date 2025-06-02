using GoalManager.Core.GoalManagement.Specifications;
using GoalManager.Core.GoalManagement;
using GoalManager.Core;

namespace GoalManager.UseCases.GoalManagement.UpdateGoalStatus;

public sealed class UpdateGoalStatusCommandHandler(IRepository<GoalSet> goalSetRepository)
    : ICommandHandler<UpdateGoalStatusCommand, Result>
{
  public async Task<Result> Handle(UpdateGoalStatusCommand request, CancellationToken cancellationToken)
  {
    var goalSet = await goalSetRepository.SingleOrDefaultAsync(
        new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken);

    if (goalSet == null)
      return Result.Error($"GoalSet not found: {request.GoalSetId}");

    var result = goalSet.UpdateGoalStatus(request.GoalId, request.Status, request.Comment);

    if (!result.IsSuccess)
      return result.ToResult();

    await goalSetRepository.UpdateAsync(goalSet, cancellationToken);

    return Result.Success(result);
  }
}
