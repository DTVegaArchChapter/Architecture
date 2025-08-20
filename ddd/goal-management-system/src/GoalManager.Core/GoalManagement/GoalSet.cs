using GoalManager.Core.GoalManagement.Events;

namespace GoalManager.Core.GoalManagement;

public class GoalSet : EntityBase, IAggregateRoot
{
  private const int MaxGoalCount = 10;

#pragma warning disable CS8618 // Required by Entity Framework
  private GoalSet() { }
#pragma warning restore CS8618

  private GoalSet(int teamId, int periodId, int userId)
  {
    TeamId = teamId;
    PeriodId = periodId;
    UserId = userId;
  }

  private readonly IList<Goal> _goals = [];

  public int UserId { get; private set; }
  public int PeriodId { get; private set; }
  public int TeamId { get; private set; }

  public GoalSetStatus? Status { get; private set; }

  public IReadOnlyCollection<Goal> Goals => _goals.AsReadOnly();

  public GoalPeriod GoalPeriod { get; private set; } = null!;

  // Optimistic concurrency token
  public byte[] RowVersion { get; private set; } = [];

  public static Result<GoalSet> Create(int teamId, int periodId, int userId)
  {
    var goalSet = new GoalSet(teamId, periodId, userId);

    goalSet.RegisterDomainEvent(new GoalSetCreatedEvent(teamId, periodId, userId));

    return goalSet;
  }

  public Result AddGoal(string title, GoalType goalType, GoalValue goalValue, int percentage)
  {
    if (Status == GoalSetStatus.WaitingForApproval || Status == GoalSetStatus.Approved)
    {
      return Result.Error($"Cannot add goals to goal set. Because goal set status is {GetStatusName()}");
    }

    if (Goals.Count >= MaxGoalCount)
    {
      return Result.Error($"Goals count cannot be bigger than {MaxGoalCount}");
    }

    var createGoalResult = Goal.Create(Id, title, goalType, goalValue, percentage);
    if (!createGoalResult.IsSuccess)
    {
      return createGoalResult.ToResult();
    }

    var totalPercentage = GetGoalsTotalPercentage();
    if (totalPercentage + percentage > 100)
    {
      return Result.Error($"Total percentage of goals cannot exceed 100. Current total percentage is {totalPercentage}");
    }

    _goals.Add(createGoalResult.Value);

    RegisterDomainEvent(new GoalAddedEvent(Id, title));

    return Result.Success();
  }

  public Result UpdateGoal(int goalId, string title, GoalType goalType, GoalValueType goalValueType, int percentage)
  {
    var goal = _goals.SingleOrDefault(x => x.Id == goalId);
    if (goal == null)
    {
      return Result.Error($"Goal not found for id: {goalId}");
    }

    if (Status == GoalSetStatus.WaitingForApproval || Status == GoalSetStatus.Approved)
    {
      return Result.Error($"Cannot update goal. Because goal set status is {GetStatusName()}");
    }

    var goalValueResult = GoalValue.Create(goal.GoalValue.MinValue, goal.GoalValue.MidValue, goal.GoalValue.MaxValue, goalValueType);
    if (!goalValueResult.IsSuccess)
    {
      return goalValueResult.ToResult();
    }

    var totalPercentage = GetGoalsTotalPercentage();
    if ((totalPercentage - goal.Percentage) + percentage > 100)
    {
      return Result.Error($"Total percentage of goals cannot exceed 100. Current total percentage is {totalPercentage}");
    }

    var result = goal.Update(title, goalType, goalValueResult.Value, percentage);
    if (!result.IsSuccess)
    {
      return result;
    }

    RegisterDomainEvent(new GoalUpdatedEvent(Id, title, goalType, goalValueResult.Value, percentage));

    return result;
  }

  public Result UpdateGoalProgress(int goalId, int actualValue, string? comment)
  {
    var goal = _goals.SingleOrDefault(g => g.Id == goalId);
    if (goal == null)
    {
      return Result.Error($"Goal not found for id: {goalId}");
    }

    if (!IsOpenForGoalChanges())
    {
      return Result.Error($"Goal set is not open for goal changes. Because goal set status is {GetStatusName()}");
    }

    var result = goal.AddProgress(TeamId, UserId, actualValue, comment);
    if (!result.IsSuccess)
    {
      return result;
    }

    RegisterDomainEvent(new GoalProgressUpdatedEvent(TeamId, goalId, goal.Title, UserId, actualValue));

    return result;
  }

  public Result ApproveGoalProgress(int goalId)
  {
    var goal = _goals.FirstOrDefault(g => g.Id == goalId);
    if (goal == null)
    {
      return Result.Error($"Goal not found for id: {goalId}");
    }

    var result = goal.UpdateProgressStatus(GoalProgressStatus.Approved);
    if (!result.IsSuccess)
    {
      return result;
    }

    RegisterDomainEvent(new GoalProgressApprovedEvent(goalId));
    return result;
  }

  public Result RejectGoalProgress(int goalId)
  {
    var goal = _goals.FirstOrDefault(g => g.Id == goalId);
    if (goal == null)
    {
      return Result.Error($"Goal not found for id: {goalId}");
    }

    var result = goal.UpdateProgressStatus(GoalProgressStatus.Rejected);
    if (!result.IsSuccess)
    {
      return result;
    }

    RegisterDomainEvent(new GoalProgressRejectedEvent(goalId));
    return result;
  }

  public Result SendToApproval()
  {
    if (!_goals.All(x => x.GoalProgress != null && x.GoalProgress.Status == GoalProgressStatus.Approved))
    {
      return Result.Error("Cannot send goal set to approval if not all goals are approved");
    }

    if (_goals.Sum(x => x.Percentage) != 100)
    {
      return Result.Error("Cannot send goal set to approval if sum of all goal percentages is not 100%");
    }

    Status = GoalSetStatus.WaitingForApproval;

    RegisterDomainEvent(new GoalSetSentToApprovalEvent(Id, TeamId, UserId));

    return Result.Success();
  }

  public Result Approve(int userId)
  {
    if (Status != GoalSetStatus.WaitingForApproval)
    {
      return Result.Error($"Cannot approve goal set. Current status is {GetStatusName()}");
    }

    if (!_goals.All(x => x.GoalProgress != null && x.GoalProgress.Status == GoalProgressStatus.Approved))
    {
      return Result.Error("Cannot send goal set to approval if not all goals are approved");
    }

    if (_goals.Sum(x => x.Percentage) != 100)
    {
      return Result.Error("Cannot send goal set to approval if sum of all goal percentages is not 100%");
    }

    Status = GoalSetStatus.Approved;

    RegisterDomainEvent(new GoalSetApprovedEvent(Id, TeamId, userId));

    return Result.Success();
  }

  public Result Reject(int userId)
  {
    if (Status != GoalSetStatus.WaitingForApproval)
    {
      return Result.Error($"Cannot reject goal set. Current status is {Status?.Name ?? "None"}");
    }

    Status = GoalSetStatus.None;

    RegisterDomainEvent(new GoalSetRejectedEvent(Id, TeamId, userId));

    return Result.Success();
  }

  public bool IsOpenForGoalChanges()
  {
    return Status is null || Status == GoalSetStatus.None;
  }

  public string GetStatusName()
  {
    return Status?.Name ?? "New";
  }

  private int GetGoalsTotalPercentage()
  {
    return _goals.Sum(x => x.Percentage);
  }
}
