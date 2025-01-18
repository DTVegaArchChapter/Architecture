namespace GoalManager.UseCases.Identity.GetUserLookup;

internal sealed class GetUserLookupQueryHandler(IIdentityRepository identityRepository) : IQueryHandler<GetUserLookupQuery, Result<List<UserLookupDto>>>
{
  public async Task<Result<List<UserLookupDto>>> Handle(GetUserLookupQuery request, CancellationToken cancellationToken)
  {
    return await identityRepository.GetUserLookup().ConfigureAwait(false);
  }
}
