using GoalManager.Core.Organisation.Specifications;

namespace GoalManager.Core.Organisation;

public interface IOrganisationService
{
  Task<Result<int>> CreateOrganisation(string name, CancellationToken cancellationToken = default);
}

public sealed class OrganisationService(IRepository<Organisation> repository) : IOrganisationService
{
  public async Task<Result<int>> CreateOrganisation(string name, CancellationToken cancellationToken = default)
  {
    var createResult = Core.Organisation.Organisation.Create(name);
    if (!createResult.IsSuccess)
    {
      return createResult.ToResult();
    }

    var organisation = createResult.Value;

    if (await repository.AnyAsync(new OrganisationByNameSpec(name), cancellationToken))
    {
      return Result<int>.Error($"Organisation with name '{name}' already exists");
    }

    await repository.AddAsync(organisation, cancellationToken).ConfigureAwait(false);
    await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    return Result.Success(organisation.Id);
  }
}
