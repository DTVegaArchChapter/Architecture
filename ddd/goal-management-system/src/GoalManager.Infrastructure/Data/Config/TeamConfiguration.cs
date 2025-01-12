using GoalManager.Core.Organisation;

namespace GoalManager.Infrastructure.Data.Config;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
  public void Configure(EntityTypeBuilder<Team> builder)
  {
    builder.HasKey(p => p.Id);

    builder.Property(p => p.Name)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();

    builder
      .HasMany(p => p.TeamMembers)
      .WithOne(p => p.Team)
      .HasForeignKey(p => p.TeamId);

    builder
      .HasOne(c => c.Organisation)
      .WithMany(p => p.Teams)
      .HasForeignKey(p => p.OrganisationId);

    builder.HasIndex(p => p.Name).IsUnique();
  }
}
