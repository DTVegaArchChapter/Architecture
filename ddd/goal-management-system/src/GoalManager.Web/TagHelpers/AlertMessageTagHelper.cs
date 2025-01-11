using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GoalManager.Web.TagHelpers;

[HtmlTargetElement("alert-messages")]
public sealed class AlertMessageTagHelper : TagHelper
{
  public IEnumerable<string>? ErrorMessages { get; set; }

  public IEnumerable<string>? SuccessMessages { get; set; }

  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    if ((ErrorMessages == null || !ErrorMessages.Any()) &&
        (SuccessMessages == null || !SuccessMessages.Any()))
    {
      output.SuppressOutput();
      return;
    }

    output.TagName = "div";
    output.Attributes.SetAttribute("role", "alert");
    output.Content.Clear();

    if (ErrorMessages != null)
    {
      foreach (var error in ErrorMessages)
      {
        var errorDiv = $"<div class=\"alert alert-danger\" role=\"alert\">{error}</div>";
        output.Content.AppendHtml(errorDiv);
      }
    }

    if (SuccessMessages != null)
    {
      foreach (var success in SuccessMessages)
      {
        var successDiv = $"<div class=\"alert alert-success\" role=\"alert\">{success}</div>";
        output.Content.AppendHtml(successDiv);
      }
    }
  }
}
