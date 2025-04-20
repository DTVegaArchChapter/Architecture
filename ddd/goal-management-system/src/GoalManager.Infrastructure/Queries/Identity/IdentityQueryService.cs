using GoalManager.Infrastructure.Identity;
using GoalManager.UseCases.Identity.GetUserLookup;
using GoalManager.UseCases.Identity;

namespace GoalManager.Infrastructure.Queries.Identity;

internal sealed class IdentityQueryService(IdentityDbContext identityDbContext) : IIdentityQueryService
{
  public Task<List<UserLookupDto>> GetUserLookup()
  {
    return identityDbContext.Users.AsNoTracking().OrderByDescending(x => x.UserName)
             .Select(x => new UserLookupDto(x.Id, x.UserName!)).ToListAsync();
  }

  public Task<string?> GetUserName(int id)
  {
    return identityDbContext.Users.AsNoTracking().Where(x => x.Id == id).Select(x => x.UserName)
             .SingleOrDefaultAsync();
  }

  public Task<List<string>> GetUserEmails(IList<int> userIds)
  {
    return identityDbContext.Users.AsNoTracking().Where(x => userIds.Contains(x.Id)).Select(x => x.Email!).ToListAsync();
  }
}
