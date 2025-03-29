namespace GoalManager.Core.Organisation.Specifications;

public sealed class OrganisationByNameSpec : Specification<Organisation>
{
  public OrganisationByNameSpec(string name) => Query.Where(x => x.OrganisationName.Value == name);
}
