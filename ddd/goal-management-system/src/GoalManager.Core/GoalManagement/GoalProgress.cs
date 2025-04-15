
namespace GoalManager.Core.GoalManagement;
public class GoalProgress : EntityBase
{
  private GoalProgress()
  {
    
  }

  public GoalProgress(int goalId, int actualValue, string? comment, GoalProgressStatus status)
  {
    GoalId = goalId;
    ActualValue = actualValue;
    Comment = comment;
    Status = status;
  }

  public int GoalId { get; private set; }
  public Goal Goal { get; private set; } = null!;
  public int ActualValue { get; private set; }
  public string? Comment { get; private set; }
  public GoalProgressStatus Status { get; set; } = null!;

  public static Result<GoalProgress> Create(int goalId, int actualValue, string? comment, GoalProgressStatus goalProgressStatus)
  {
    var goalProgress = new GoalProgress(goalId, actualValue, comment, goalProgressStatus);

    return goalProgress;
  }
}
