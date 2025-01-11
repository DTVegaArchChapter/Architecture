using System.ComponentModel.DataAnnotations;

namespace GoalManager.Web.ViewModels.Organisation;

public class CreateOrganisationViewModel
{
  [Required]
  public string Name { get; set; } = null!;
}
