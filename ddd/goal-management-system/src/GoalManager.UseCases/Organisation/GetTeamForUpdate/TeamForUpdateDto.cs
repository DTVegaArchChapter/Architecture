using System.ComponentModel.DataAnnotations;

namespace GoalManager.UseCases.Organisation.GetTeamForUpdate;

public record TeamForUpdateDto(int Id, string Name, IEnumerable<TeamMemberDto> TeamMembers);
