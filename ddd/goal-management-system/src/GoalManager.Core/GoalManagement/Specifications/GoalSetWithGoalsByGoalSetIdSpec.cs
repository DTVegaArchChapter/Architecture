namespace GoalManager.Core.GoalManagement.Specifications;

public sealed class GoalSetWithGoalsByGoalSetIdSpec : SingleResultSpecification<GoalSet>
{
  public GoalSetWithGoalsByGoalSetIdSpec(int goalSetId) => Query.Include(x => x.Goals).ThenInclude(x => x.GoalProgressHistory).Where(x => x.Id == goalSetId);
}
