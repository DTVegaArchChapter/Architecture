using GoalManager.Core.Organisation.Specifications;

namespace GoalManager.UseCases.Organisation.GetForUpdate;

internal sealed class GetOrganisationForUpdateQueryHandler(IRepository<Core.Organisation.Organisation> repository) : IQueryHandler<GetOrganisationForUpdateQuery, Result<OrganisationForUpdateDto>>
{
  public async Task<Result<OrganisationForUpdateDto>> Handle(GetOrganisationForUpdateQuery request, CancellationToken cancellationToken)
  {
    var organisation = await repository.SingleOrDefaultAsync(new OrganisationWithTeamsByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);
    if (organisation == null)
    {
      return Result<OrganisationForUpdateDto>.Error("Organisation not found");
    }

    return new OrganisationForUpdateDto
           {
             Id = organisation.Id,
             OrganisationName = organisation.OrganisationName.Value,
             Teams = organisation.Teams.Select(x => new OrganisationTeamDto
                                                    {
                                                      Id = x.Id,
                                                      Name = x.Name.Value,
                                                      MemberCount = x.TeamMembers.Count
                                                    }).ToList()
           };
  }
}
