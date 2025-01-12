using Ardalis.GuardClauses;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GoalManager.Web.Common;

public class PageModelBase : PageModel
{
  private const string TempDataSuccessMessageKey = "__SuccessMessage";

  public List<string> ErrorMessages { get; protected set; } = [];

  public List<string> SuccessMessages { get; protected set; } = [];

  public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
  {
    base.OnPageHandlerExecuting(context);

    if (TempData.TryGetValue(TempDataSuccessMessageKey, out var value) && value != null)
    {
      var message = Convert.ToString(value);
      if (!string.IsNullOrWhiteSpace(message))
      {
        SuccessMessages.Add(message);
      }
    }
  }

  protected void AddResultMessages(Result result)
  {
    AddMessages(result.Errors, result.SuccessMessage);
  }

  protected void AddResultMessages<T>(Result<T> result)
  {
    AddMessages(result.Errors, result.SuccessMessage);
  }

  protected void AddSuccessMessage(string message)
  {
    Guard.Against.NullOrWhiteSpace(message);

    SuccessMessages.Add(message);
  }

  protected RedirectToPageResult RedirectToPageWithSuccessMessage(string? pageName, string message)
  {
    TempData[TempDataSuccessMessageKey] = message;
    return RedirectToPage(pageName);
  }

  private void AddMessages(IEnumerable<string> errors, string successMessage)
  {
    ErrorMessages.AddRange(errors);

    if (!string.IsNullOrWhiteSpace(successMessage))
    {
      SuccessMessages.Add(successMessage);
    }
  }

}
