namespace GoalManager.UseCases.GoalManagement.CalculateGoalPointCommand;

public class CalculateGoalPointCommand(int goalSetId, int goalId) : ICommand<Result>
{
  public int GoalSetId { get; } = goalSetId;
  public int GoalId { get; } = goalId;
}
