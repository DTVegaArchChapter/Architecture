namespace GoalManager.UseCases.Organisation.GetForUpdate;

internal sealed class GetOrganisationForUpdateQueryHandler(IRepository<Core.Organisation.Organisation> repository) : IQueryHandler<GetOrganisationForUpdateQuery, Result<OrganisationForUpdateDto>>
{
  public async Task<Result<OrganisationForUpdateDto>> Handle(GetOrganisationForUpdateQuery request, CancellationToken cancellationToken)
  {
    var organisation = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
    if (organisation == null)
    {
      return Result<OrganisationForUpdateDto>.Error("Organisation not found");
    }

    return new OrganisationForUpdateDto
           {
             OrganisationName = organisation.Name,
             Teams = organisation.Teams.Select(x => new OrganisationTeamDto
                                                    {
                                                      Id = x.Id,
                                                      Name = x.Name,
                                                      MemberCount = x.TeamMembers.Count
                                                    }).ToList()
           };
  }
}
