using System.ComponentModel.DataAnnotations;

namespace GoalManager.UseCases.Organisation.GetForUpdate;

public sealed class OrganisationForUpdateDto
{
  public int Id { get; set; }

  [Required]
  public string OrganisationName { get; set; } = null!;

  public IList<OrganisationTeamDto> Teams { get; set; } = new List<OrganisationTeamDto>();
}
