using GoalManager.Core.Organisation;

namespace GoalManager.UseCases.Organisation.RemoveTeamMember;

internal sealed class RemoveTeamMemberCommandHandler(IOrganisationService organisationService) : ICommandHandler<RemoveTeamMemberCommand, Result>
{
  public async Task<Result> Handle(RemoveTeamMemberCommand request, CancellationToken cancellationToken)
  {
    return await organisationService
             .RemoveTeamMember(request.OrganisationId, request.TeamId, request.UserId, cancellationToken)
             .ConfigureAwait(false);
  }
}
