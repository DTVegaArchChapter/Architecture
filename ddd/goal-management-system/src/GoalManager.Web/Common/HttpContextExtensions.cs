using System.Security.Claims;
using GoalManager.Web.Common.Identity;

namespace GoalManager.Web.Common;

public static class HttpContextExtensions
{
  public static UserContext GetUserContext(this HttpContext context)
  {
    var userId = 0;
    var userIdStringValue = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    if (!string.IsNullOrWhiteSpace(userIdStringValue) && int.TryParse(userIdStringValue, out var result))
    {
      userId = result;
    }


    return new UserContext {Id = userId, UserName = context.User.Identity?.Name};
  }
}
