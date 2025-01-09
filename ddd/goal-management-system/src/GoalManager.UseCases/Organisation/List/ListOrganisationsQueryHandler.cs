namespace GoalManager.UseCases.Organisation.List;

public class ListOrganisationsQueryHandler(IOrganisationQueryService organisationQueryService)
  : IQueryHandler<ListOrganisationsQuery, Result<List<OrganisationListItemDto>>>
{
  public async Task<Result<List<OrganisationListItemDto>>> Handle(ListOrganisationsQuery request, CancellationToken cancellationToken)
  {
    var result = await organisationQueryService.ListAsync(request.Skip, request.Take).ConfigureAwait(false);

    return Result.Success(result);
  }
}
