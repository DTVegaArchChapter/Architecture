using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GoalManager.Web.Pages;

[Authorize]
public class IndexModel : PageModel;
