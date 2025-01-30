namespace GoalManager.Core.GoalManagement;
internal class GoalPeriod : EntityBase, IAggregateRoot
{
  public int TeamId { get; private set; }
  public int Year { get; private set; }

  private readonly IList<Goal> _teamGoals = [];
  public IReadOnlyCollection<Goal> TeamGoals => _teamGoals.AsReadOnly();
}
