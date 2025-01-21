namespace GoalManager.UseCases.Organisation.AddTeamMember;

public record AddTeamMemberCommand(int OrganisationId, int TeamId, int UserId, int MemberTypeId) : ICommand<Result>;
