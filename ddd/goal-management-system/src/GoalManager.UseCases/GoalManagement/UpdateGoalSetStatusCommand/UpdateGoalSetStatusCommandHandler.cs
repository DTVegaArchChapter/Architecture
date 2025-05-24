
using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.UpdateGoalSetStatusCommand;
public class UpdateGoalSetStatusCommand: ICommand<Result>
{
  public int GoalSetId { get; set; }
  public GoalSetStatus? Status { get; set; }
  public UpdateGoalSetStatusCommand(int goalSetId, GoalSetStatus? status)
  {
    GoalSetId = goalSetId;
    Status = status;
  }



  public class UpdateGoalSetStatusCommandHandler(IRepository<GoalSet> goalSetRepository) : ICommandHandler<UpdateGoalSetStatusCommand, Result>
  {
    public async Task<Result> Handle(UpdateGoalSetStatusCommand request, CancellationToken cancellationToken)
    {
      var goalSet = await goalSetRepository.SingleOrDefaultAsync(
        new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId), cancellationToken);

      if (goalSet == null)
        return Result.Error($"GoalSet not found: {request.GoalSetId}");

      if(request.Status == null)
        return Result.Error("Status cannot be null");

      var result = goalSet.UpdateStatus(request.Status);

      if (!result.IsSuccess)
        return result.ToResult();

      await goalSetRepository.UpdateAsync(goalSet, cancellationToken);

      return Result.Success(result);
    }
  }
}
