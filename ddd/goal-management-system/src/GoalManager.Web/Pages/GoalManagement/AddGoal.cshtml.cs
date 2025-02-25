using GoalManager.Core.GoalManagement;
using System.ComponentModel.DataAnnotations;

using GoalManager.UseCases.GoalManagement.GetGoalTypeLookup;
using GoalManager.Web.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using GoalManager.UseCases.GoalManagement.GetGoalValueTypeLookup;

namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class AddGoalModel(IMediator mediator) : PageModelBase
{
  [BindProperty]
  [Required(ErrorMessage = "Goal Title is required.")]
  public string Title { get; set; } = null!;

  [BindProperty]
  [Required(ErrorMessage = "Goal Type is required.")]
  public int? GoalTypeId { get; set; }

  [BindProperty]
  [Required(ErrorMessage = "Goal Value Type is required.")]
  public int? GoalValueTypeId { get; set; }

  [BindProperty]
  [Required(ErrorMessage = "Min Value is required.")]
  [Range(0, int.MaxValue, ErrorMessage = "Min Value must be a non-negative integer.")]
  public int MinValue { get; set; }

  [BindProperty]
  [Required(ErrorMessage = "Mid Value is required.")]
  [Range(0, int.MaxValue, ErrorMessage = "Mid Value must be a non-negative integer.")]
  public int MidValue { get; set; }

  [BindProperty]
  [Required(ErrorMessage = "Max Value is required.")]
  [Range(0, int.MaxValue, ErrorMessage = "Max Value must be a non-negative integer.")]
  public int MaxValue { get; set; }

  public List<SelectListItem> GoalTypeOptions { get; set; } = new();
  public List<SelectListItem> GoalValueTypeOptions { get; set; } = new();

  public async Task<IActionResult> OnGetAsync()
  {
    var goalTypesResult = await mediator.Send(new GetGoalTypeLookupQuery()).ConfigureAwait(false);

    AddResultMessages(goalTypesResult);

    if (goalTypesResult.IsSuccess)
    {
      var goalTypes = goalTypesResult.Value.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
      GoalTypeOptions.Add(new SelectListItem("Choose..", string.Empty));
      GoalTypeOptions.AddRange(goalTypes);
    }

    var goalValueTypesResult = await mediator.Send(new GetGoalValueTypeLookupQuery()).ConfigureAwait(false);

    AddResultMessages(goalValueTypesResult);

    if (goalValueTypesResult.IsSuccess)
    {
      var goalValueTypes = goalValueTypesResult.Value.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
      GoalValueTypeOptions.Add(new SelectListItem("Choose..", string.Empty));
      GoalValueTypeOptions.AddRange(goalValueTypes);
    }

    return Page();
  }
}
