
using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.CalculateAllGoalPointCommand;
public class CalculateAllGoalPointCommand: ICommand<Result>
{
  public int GoalSetId { get; set; }

  public CalculateAllGoalPointCommand(int goalSetId)
  {
    GoalSetId = goalSetId;
  }

  public class CalculateAllGoalPointCommandHandler(IRepository<GoalSet> _goalSetRepository) : ICommandHandler<CalculateAllGoalPointCommand, Result>
  {
    public async Task<Result> Handle(CalculateAllGoalPointCommand request, CancellationToken cancellationToken)
    {
      var goalSet = await _goalSetRepository.SingleOrDefaultAsync(
      new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken);

      if (goalSet == null)
        return Result.Error($"GoalSet not found: {request.GoalSetId}");


      var result = goalSet.CalculateAllGoalPoint();

      if (!result.IsSuccess)
        return result.ToResult();

      await _goalSetRepository.UpdateAsync(goalSet, cancellationToken);

      return Result.Success(result);
    }
  }
}
