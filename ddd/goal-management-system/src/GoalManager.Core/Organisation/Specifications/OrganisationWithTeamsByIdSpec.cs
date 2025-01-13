namespace GoalManager.Core.Organisation.Specifications;

public sealed class OrganisationWithTeamsByIdSpec : SingleResultSpecification<Organisation>
{
  public OrganisationWithTeamsByIdSpec(int id) => Query.Where(x => x.Id == id).Include(x => x.Teams).ThenInclude(x => x.TeamMembers);
}
