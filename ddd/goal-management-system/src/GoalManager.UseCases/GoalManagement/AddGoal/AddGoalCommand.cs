using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.AddGoal;

public record AddGoalCommand(int GoalSetId, string Title, GoalType GoalType, int MinValue, int MidValue, int MaxValue, GoalValueType GoalValueType, int Percentage) : ICommand<Result<(int TeamId, int PeriodId)>>;
