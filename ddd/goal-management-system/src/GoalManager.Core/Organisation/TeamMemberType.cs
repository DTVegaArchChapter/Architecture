namespace GoalManager.Core.Organisation;

public class TeamMemberType : SmartEnum<TeamMemberType>
{
  public static readonly TeamMemberType TeamLeader = new(nameof(TeamLeader), 1);
  public static readonly TeamMemberType Member = new(nameof(Member), 2);

  protected TeamMemberType(string name, int value) : base(name, value) { }
}
