using GoalManager.Core.Organisation.Specifications;
using MediatR;

namespace GoalManager.Core.Organisation;

public interface IOrganisationService
{
  Task<Result<int>> CreateOrganisation(string name, CancellationToken cancellationToken = default);

  Task<Result> DeleteOrganisation(int id, CancellationToken cancellationToken = default);

  Task<Result> UpdateOrganisation(int id, string name, CancellationToken cancellationToken = default);
}

public sealed class OrganisationService(IRepository<Organisation> repository) : IOrganisationService
{
  public async Task<Result<int>> CreateOrganisation(string name, CancellationToken cancellationToken = default)
  {
    var createResult = Organisation.Create(name);
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

    return Result.Success(organisation.Id);
  }

  public async Task<Result> UpdateOrganisation(int id, string name, CancellationToken cancellationToken = default)
  {
    var organisation = await repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
    if (organisation == null)
    {
      return Result.Error("Organisation not found");
    }

    if (await repository.AnyAsync(new OrganisationByNameSpec(name), cancellationToken))
    {
      return Result.Error($"Organisation with name '{name}' already exists");
    }

    organisation.Update(name);

    await repository.UpdateAsync(organisation, cancellationToken).ConfigureAwait(false);

    return Result.Success();
  }

  public async Task<Result> DeleteOrganisation(int id, CancellationToken cancellationToken = default)
  {
    var organisation = await repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
    if (organisation == null)
    {
      return Result.Error("Organisation is not found");
    }

    if (organisation.Teams.Any())
    {
      return Result.Error("You cannot delete the organisation because there are teams created under the organisation");
    }

    organisation.Delete();

    await repository.DeleteAsync(organisation, cancellationToken).ConfigureAwait(false);

    return Result.SuccessWithMessage("Organisation is deleted");
  }
}
