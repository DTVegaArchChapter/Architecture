using System.ComponentModel.DataAnnotations;

namespace GoalManager.UseCases.Organisation.GetForUpdate;

public sealed class OrganisationForUpdateDto
{
  [Required]
  public string OrganisationName { get; set; } = null!;

  public IList<OrganisationTeamDto> Teams { get; set; } = new List<OrganisationTeamDto>();
}
