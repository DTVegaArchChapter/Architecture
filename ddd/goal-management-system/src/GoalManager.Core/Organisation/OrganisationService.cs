﻿using GoalManager.Core.Organisation.Specifications;

namespace GoalManager.Core.Organisation;

public interface IOrganisationService
{
  Task<Result<int>> CreateOrganisation(string name, CancellationToken cancellationToken = default);

  Task<Result> DeleteOrganisation(int id, CancellationToken cancellationToken = default);

  Task<Result> UpdateOrganisation(int id, string name, CancellationToken cancellationToken = default);

  Task<Result> AddNewTeam(int organisationId, string teamName, CancellationToken cancellationToken = default);

  Task<Result> UpdateTeam(int organisationId, int teamId, string teamName, CancellationToken cancellationToken = default);

  Task<Result> DeleteTeam(int organisationId, int teamId, CancellationToken cancellationToken = default);

  Task<Result> AddTeamMember(int organisationId, int teamId, int userId, string memberName, CancellationToken cancellationToken = default);
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
    var organisation = await GetOrganisation(id, cancellationToken).ConfigureAwait(false);
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
    var organisation = await GetOrganisation(id, cancellationToken).ConfigureAwait(false);
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

  public async Task<Result> AddNewTeam(int organisationId, string teamName, CancellationToken cancellationToken = default)
  {
    var organisation = await GetOrganisation(organisationId, cancellationToken).ConfigureAwait(false);
    if (organisation == null)
    {
      return Result.Error("Organisation is not found");
    }

    var teamResult = organisation.AddTeam(teamName);
    if (!teamResult.IsSuccess)
    {
      return teamResult;
    }

    await repository.UpdateAsync(organisation, cancellationToken).ConfigureAwait(false);

    return Result.SuccessWithMessage("Team is created");
  }

  public async Task<Result> UpdateTeam(int organisationId, int teamId, string teamName, CancellationToken cancellationToken = default)
  {
    var organisation = await GetOrganisation(organisationId, cancellationToken).ConfigureAwait(false);
    if (organisation == null)
    {
      return Result.Error("Organisation is not found");
    }

    var teamResult = organisation.UpdateTeam(teamId, teamName);
    if (!teamResult.IsSuccess)
    {
      return teamResult;
    }

    await repository.UpdateAsync(organisation, cancellationToken).ConfigureAwait(false);

    return Result.Success();
  }

  public async Task<Result> DeleteTeam(int organisationId, int teamId, CancellationToken cancellationToken = default)
  {
    var organisation = await GetOrganisation(organisationId, cancellationToken).ConfigureAwait(false);
    if (organisation == null)
    {
      return Result.Error("Organisation is not found");
    }

    var result = organisation.DeleteTeam(teamId);
    if (!result.IsSuccess)
    {
      return result;
    }

    await repository.UpdateAsync(organisation, cancellationToken).ConfigureAwait(false);

    return Result.Success();
  }

  public async Task<Result> AddTeamMember(int organisationId, int teamId, int userId, string memberName, CancellationToken cancellationToken = default)
  {
    var organisation = await GetOrganisation(organisationId, cancellationToken).ConfigureAwait(false);
    if (organisation == null)
    {
      return Result.Error("Organisation is not found");
    }

    var teamResult = organisation.AddTeamMember(teamId, memberName, userId);
    if (!teamResult.IsSuccess)
    {
      return teamResult;
    }

    await repository.UpdateAsync(organisation, cancellationToken).ConfigureAwait(false);

    return Result.Success();
  }

  private Task<Organisation?> GetOrganisation(int id, CancellationToken cancellationToken)
  {
    return repository.SingleOrDefaultAsync(new OrganisationWithTeamsByIdSpec(id), cancellationToken);
  }
}
