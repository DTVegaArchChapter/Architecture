namespace GoalManager.Core.GoalManagement;
internal class Goal : EntityBase
{
  public string Title { get; private set; } = null!;
  public int MinValue { get; private set; }
  public int MidValue { get; private set; }
  public int MaxValue { get; private set; }
  public GoalType GoalType { get; private set; } = null!;
  public GoalValueType GoalValueType { get; private set; } = null!;
  public int ActualValue { get; private set; }
  public int GoalSetId { get; private set; }
  public GoalSet GoalSet { get; private set; } = null!;

  private readonly IList<GoalProgress> _goalProgress = [];
  public IReadOnlyCollection<GoalProgress> GoalProgress => _goalProgress.AsReadOnly();
}
