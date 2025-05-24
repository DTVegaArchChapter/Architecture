namespace GoalManager.UseCases.GoalManagement.CalculateCharacterPointCommand;

public class CalculateCharacterPointCommand : ICommand<Result>
{
  public int TeamId { get; set; }

  public CalculateCharacterPointCommand(int teamId)
  {
    TeamId = teamId;
  }
}
