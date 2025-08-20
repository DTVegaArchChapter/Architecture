using GoalManager.Core.Organisation;

namespace GoalManager.Infrastructure.Data.Config.Organisation;

public class OrganisationConfiguration : IEntityTypeConfiguration<Core.Organisation.Organisation>
{
  public void Configure(EntityTypeBuilder<Core.Organisation.Organisation> builder)
  {
    builder.HasKey(p => p.Id);

    builder.Property(p => p.Name)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .HasConversion(x => x.Value, x => new OrganisationName(x))
      .IsRequired();

    // Configure optimistic concurrency token for Organisation
    builder.Property(p => p.RowVersion)
      .IsRowVersion()
      .ValueGeneratedOnAddOrUpdate();

    builder
      .HasMany(c => c.Teams)
      .WithOne(p => p.Organisation)
      .HasForeignKey(p => p.OrganisationId);

    builder.HasIndex(p => p.Name).IsUnique();
  }
}
