using GoalManager.Core.Organisation;
using GoalManager.UseCases.Identity;

namespace GoalManager.UseCases.Organisation.AddTeamMember;

internal sealed class AddTeamMemberCommandHandler(IOrganisationService organisationService, IIdentityRepository identityRepository) : ICommandHandler<AddTeamMemberCommand, Result>
{
  public async Task<Result> Handle(AddTeamMemberCommand request, CancellationToken cancellationToken)
  {
    var memberName = await identityRepository.GetUserName(request.UserId).ConfigureAwait(false) ?? "unknown";
    return await organisationService
             .AddTeamMember(request.OrganisationId, request.TeamId, request.UserId, memberName, cancellationToken)
             .ConfigureAwait(false);
  }
}
