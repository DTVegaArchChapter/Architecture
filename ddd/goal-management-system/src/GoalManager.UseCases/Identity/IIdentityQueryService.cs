using GoalManager.UseCases.Identity.GetUserLookup;

namespace GoalManager.UseCases.Identity;

public interface IIdentityQueryService
{
  Task<List<UserLookupDto>> GetUserLookup();

  Task<string?> GetUserName(int id);

  Task<List<string>> GetUserEmails(IList<int> userIds);
}
