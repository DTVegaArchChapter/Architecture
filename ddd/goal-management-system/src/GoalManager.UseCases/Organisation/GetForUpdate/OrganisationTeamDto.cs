namespace GoalManager.UseCases.Organisation.GetForUpdate;

public sealed class OrganisationTeamDto
{
  public int Id { get; set; }

  public string Name { get; set; } = null!;

  public int MemberCount { get; set; }
}
