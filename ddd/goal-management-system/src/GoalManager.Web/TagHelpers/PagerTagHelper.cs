using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GoalManager.Web.TagHelpers;

[HtmlTargetElement("pager")]
public sealed class PagerTagHelper : TagHelper
{
  public long TotalItems { get; set; }
  public long ItemsPerPage { get; set; }
  public long CurrentPage { get; set; }
  public string PageUrl { get; set; } = "?page={0}";

  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    output.TagName = "nav";
    output.Attributes.SetAttribute("aria-label", "Page navigation");

    int totalPages = (int)Math.Ceiling((double)TotalItems / ItemsPerPage);

    if (totalPages <= 1)
    {
      output.SuppressOutput(); // Hide pager if there's only one page
      return;
    }

    var ul = new TagBuilder("ul");
    ul.AddCssClass("pagination");

    for (int i = 1; i <= totalPages; i++)
    {
      var li = new TagBuilder("li");
      li.AddCssClass("page-item");
      if (i == CurrentPage)
      {
        li.AddCssClass("active");
      }

      var a = new TagBuilder("a");
      a.AddCssClass("page-link");
      a.Attributes["href"] = string.Format(PageUrl, i);
      a.InnerHtml.Append(i.ToString());

      li.InnerHtml.AppendHtml(a);
      ul.InnerHtml.AppendHtml(li);
    }

    output.Content.AppendHtml(ul);
  }
}
