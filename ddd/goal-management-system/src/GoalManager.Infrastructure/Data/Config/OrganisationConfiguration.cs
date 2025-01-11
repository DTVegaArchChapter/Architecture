using GoalManager.Core.Organisation;

namespace GoalManager.Infrastructure.Data.Config;

public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
  public void Configure(EntityTypeBuilder<Organisation> builder)
  {
    builder.Property(p => p.Name)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();

    builder
      .HasMany(c => c.Teams)
      .WithOne(p => p.Organisation)
      .HasForeignKey(p => p.OrganisationId);

    builder.HasIndex(p => p.Name).IsUnique();
  }
}
