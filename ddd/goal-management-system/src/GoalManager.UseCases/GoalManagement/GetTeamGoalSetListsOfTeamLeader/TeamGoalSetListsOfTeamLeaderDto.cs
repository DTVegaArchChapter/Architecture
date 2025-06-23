namespace GoalManager.UseCases.GoalManagement.GetTeamGoalSetListsOfTeamLeader;

public sealed class TeamGoalSetListItem
{
  private List<GoalSetListItemDto> _goalSets = [];

  public int TeamId { get; set; }

  public string TeamName { get; set; } = null!;

  public List<GoalSetListItemDto> GoalSets
  {
    get => _goalSets ??= [];
    set => _goalSets = value;
  }
}
