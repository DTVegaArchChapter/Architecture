using GoalManager.UseCases.Identity.GetUserLookup;

namespace GoalManager.UseCases.Identity;

public interface IIdentityRepository
{
  Task<List<UserLookupDto>> GetUserLookup();

  Task<string?> GetUserName(int id);
}
