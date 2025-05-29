using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.CalculateGoalPointCommand;

public class CalculateGoalPointCommandHandler(IRepository<GoalSet> _goalSetRepository) : ICommandHandler<CalculateGoalPointCommand, Result>
{
  public async Task<Result> Handle(CalculateGoalPointCommand request, CancellationToken cancellationToken)
  {
    var goalSet = await _goalSetRepository.SingleOrDefaultAsync(
        new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken);

    if (goalSet == null)
      return Result.Error($"GoalSet not found: {request.GoalSetId}");


    var result = goalSet.CalculateGoalPoint(request.GoalId);

    if (!result.IsSuccess)
      return result.ToResult();

    await _goalSetRepository.UpdateAsync(goalSet, cancellationToken);

    return Result.Success(result);
  }
}
