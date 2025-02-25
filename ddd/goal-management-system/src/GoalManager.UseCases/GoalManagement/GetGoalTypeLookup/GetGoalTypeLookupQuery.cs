namespace GoalManager.UseCases.GoalManagement.GetGoalTypeLookup;

public record GetGoalTypeLookupQuery : IQuery<Result<List<GoalTypeLookupDto>>>;
