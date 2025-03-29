using System.ComponentModel.DataAnnotations;
using GoalManager.Core.GoalManagement;
using GoalManager.UseCases.GoalManagement.GetGoal;
using GoalManager.UseCases.GoalManagement.GetGoalTypeLookup;
using GoalManager.UseCases.GoalManagement.GetGoalValueTypeLookup;
using GoalManager.UseCases.GoalManagement.UpdateGoal;
using GoalManager.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class UpdateGoalModel(IMediator mediator) : PageModelBase
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
  [Required(ErrorMessage = "Percentage is required.")]
  [Range(0, int.MaxValue, ErrorMessage = "Percentage must be a non-negative integer.")]
  public int Percentage { get; set; }

  public List<SelectListItem> GoalTypeOptions { get; set; } = new();
  public List<SelectListItem> GoalValueTypeOptions { get; set; } = new();


  public async Task<IActionResult> OnGetAsync(int goalSetId, int goalId)
  {
    var goalResult = await mediator.Send(new GetGoalQuery(goalSetId, goalId)).ConfigureAwait(false);

    AddResultMessages(goalResult);

    if (goalResult.IsSuccess)
    {
      Title = goalResult.Value.Title;
      Percentage = goalResult.Value.Percentage;
      GoalValueTypeId = goalResult.Value.GoalValue.GoalValueType.Value;
      GoalTypeId = goalResult.Value.GoalType.Value;
    }

    var goalTypesResult = await mediator.Send(new GetGoalTypeLookupQuery()).ConfigureAwait(false);

    AddResultMessages(goalTypesResult);

    if (goalTypesResult.IsSuccess)
    {
      var goalTypes = goalTypesResult.Value.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
      //GoalTypeOptions.Add(new SelectListItem("Choose..", string.Empty));
      GoalTypeOptions.AddRange(goalTypes);
    }

    var goalValueTypesResult = await mediator.Send(new GetGoalValueTypeLookupQuery()).ConfigureAwait(false);

    AddResultMessages(goalValueTypesResult);

    if (goalValueTypesResult.IsSuccess)
    {
      var goalValueTypes = goalValueTypesResult.Value.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
      //GoalValueTypeOptions.Add(new SelectListItem("Choose..", string.Empty));
      GoalValueTypeOptions.AddRange(goalValueTypes);
    }

    return Page();
  }

  public async Task<IActionResult> OnPostAsync(int goalSetId, int goalId)
  {
    if (!ModelState.IsValid)
    {
      return await OnGetAsync(goalSetId, goalId).ConfigureAwait(false);
    }

    var result = await mediator.Send(
                   new UpdateGoalCommand(
                     goalSetId,
                     goalId,
                     Title,
                     GoalType.FromValue(GoalTypeId.GetValueOrDefault()),
                     GoalValueType.FromValue(GoalValueTypeId.GetValueOrDefault()),
                     Percentage));

    AddResultMessages(result);

    if (!result.IsSuccess)
    {
      return await OnGetAsync(goalSetId, goalId).ConfigureAwait(false);
    }

    return RedirectToPageWithSuccessMessage("UpdateGoal", new { result.Value.GoalSetId, result.Value.GoalId }, "Goal is updated");
  }
}
