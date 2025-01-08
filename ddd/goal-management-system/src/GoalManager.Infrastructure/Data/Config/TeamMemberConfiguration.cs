using GoalManager.Core.OrganisationAggregate;

namespace GoalManager.Infrastructure.Data.Config;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
  public void Configure(EntityTypeBuilder<TeamMember> builder)
  {
    builder.Property(p => p.Name)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();

    builder
      .HasOne(c => c.Team)
      .WithMany(p => p.TeamMembers)
      .HasForeignKey(p => p.TeamId);
  }
}
