using GoalManager.UseCases.Identity;
using GoalManager.UseCases.Identity.GetUserLookup;

namespace GoalManager.Infrastructure.Identity;

internal sealed class IdentityRepository(IdentityDbContext identityDbContext) : IIdentityRepository
{
  public async Task<List<UserLookupDto>> GetUserLookup()
  {
    return await identityDbContext.Users.AsNoTracking().OrderByDescending(x => x.UserName)
             .Select(x => new UserLookupDto(x.Id, x.UserName!)).ToListAsync().ConfigureAwait(false);
  }

  public async Task<string?> GetUserName(int id)
  {
    return await identityDbContext.Users.AsNoTracking().Where(x => x.Id == id).Select(x => x.UserName)
             .SingleOrDefaultAsync().ConfigureAwait(false);
  }
}
