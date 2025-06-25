namespace GoalManager.Core.PerformanceEvaluation.Specifications;
public sealed class GoalSetEvaluationWithGoalEvaluationsByGoalSetIdSpec : Specification<GoalSetEvaluation>, ISingleResultSpecification<GoalSetEvaluation>
{
  public GoalSetEvaluationWithGoalEvaluationsByGoalSetIdSpec(int goalSetId) => Query.Where(x => x.GoalSetId == goalSetId).Include(x => x.GoalEvaluations);
}
