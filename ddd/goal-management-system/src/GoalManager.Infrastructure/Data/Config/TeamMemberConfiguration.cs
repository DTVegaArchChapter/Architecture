using GoalManager.Core.Organisation;

namespace GoalManager.Infrastructure.Data.Config;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
  public void Configure(EntityTypeBuilder<TeamMember> builder)
  {
    builder.HasKey(p => p.Id);

    builder.Property(p => p.Name)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();

    builder
      .HasOne(c => c.Team)
      .WithMany(p => p.TeamMembers)
      .HasForeignKey(p => p.TeamId);

    builder.Property(x => x.MemberType)
      .HasConversion(
        x => x.Value,
        x => TeamMemberType.FromValue(x));
  }
}
