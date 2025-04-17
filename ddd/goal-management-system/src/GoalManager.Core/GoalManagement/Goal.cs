using Ardalis.Result;
using GoalManager.Core.GoalManagement.Events;

namespace GoalManager.Core.GoalManagement;

public class Goal : EntityBase
{
  public string Title { get; private set; }
  public GoalType GoalType { get; private set; }
  public GoalValue GoalValue { get; private set; }
  public int? ActualValue { get; private set; }
  public int GoalSetId { get; private set; }
  public int Percentage { get; private set; }

  private readonly IList<GoalProgress> _goalProgressHistory = [];
  public IReadOnlyCollection<GoalProgress> GoalProgressHistory => _goalProgressHistory.AsReadOnly();

#pragma warning disable CS8618 // Required by Entity Framework
  private Goal() { }
#pragma warning restore CS8618

  private Goal(int goalSetId, string title, GoalType goalType, GoalValue goalValue, int percentage)
  {
    Title = Guard.Against.NullOrWhiteSpace(title);
    GoalType = goalType;
    GoalValue = goalValue;
    GoalSetId = goalSetId;
    Percentage = percentage;
  }

  public Result SetActualValue(int value)
  {
    if (value > GoalValue.MaxValue)
    {
      return Result.Error("Actual value cannot be bigger than max value");
    }
    if (value < GoalValue.MinValue)
    {
      return Result.Error("Actual value cannot be less than min value");
    }
    ActualValue = value;

    return Result.Success();
  }

  internal static Result<Goal> Create(int goalSetId, string title, GoalType goalType, GoalValue goalValue, int percentage)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      return Result<Goal>.Error("Goal title is required");
    }

    var goal = new Goal(goalSetId, title, goalType, goalValue, percentage);
    goal.RegisterGoalCreatedEvent();
    return goal;
  }

  internal Result Update(string title, GoalType goalType, GoalValue goalValue, int percantage)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      return Result.Error("Goal title is required");
    }

    Title = title;
    GoalType = goalType;
    GoalValue = goalValue;
    Percentage = percantage;

    return Result.Success();
  }
  public Result AddProgress(int actualValue, string? comment, GoalProgressStatus status)
  {
    var setValueResult = SetActualValue(actualValue);
    if (!setValueResult.IsSuccess)
      return setValueResult;

    var progressResult = GoalProgress.Create(Id, actualValue, comment, status);
    if (!progressResult.IsSuccess)
      return progressResult.ToResult();

    _goalProgressHistory.Clear();
    _goalProgressHistory.Add(progressResult.Value);

    return Result.Success();
  }
  private void RegisterGoalCreatedEvent()
  {
    RegisterDomainEvent(new GoalCreatedEvent(Title));
  }

  public Result UpdateProgressStatus(GoalProgressStatus newStatus, string? comment = null)
  {
    if (!_goalProgressHistory.Any())
      return Result.Error("No progress record found to update");

    var latestProgress = _goalProgressHistory.OrderByDescending(p => p.Id).First();

    var updatedProgress = GoalProgress.Create(
        Id,
        latestProgress.ActualValue,
        comment ?? latestProgress.Comment,
        newStatus);

    if (!updatedProgress.IsSuccess)
      return updatedProgress.ToResult();

    _goalProgressHistory.Remove(latestProgress);
    _goalProgressHistory.Add(updatedProgress.Value);

    return Result.Success();
  }
}
