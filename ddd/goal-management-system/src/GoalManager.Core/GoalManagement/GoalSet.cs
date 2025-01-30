namespace GoalManager.Core.GoalManagement;
internal class GoalSet : EntityBase, IAggregateRoot
{
  private GoalSet() { }

  private readonly IList<Goal> _teamGoals = [];
  private readonly IList<Goal> _individualGoals = [];

  public int UserId { get; private set; }
  public int PeriodId { get; private set; }

  public IReadOnlyCollection<Goal> TeamGoals => _teamGoals.AsReadOnly();
  public IReadOnlyCollection<Goal> IndividualGoals => _individualGoals.AsReadOnly();
}
