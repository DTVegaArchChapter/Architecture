using GoalManager.Core.GoalManagement;

namespace GoalManager.Infrastructure.Data.Config.GoalManagement;

internal sealed class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
  public void Configure(EntityTypeBuilder<Goal> builder)
  {
    builder.HasKey(g => g.Id);

    builder.Property(g => g.Title)
      .IsRequired()
      .HasMaxLength(200);

    builder.Property(g => g.GoalType)
      .HasConversion(
        p => p.Value,
        p => GoalType.FromValue(p))
      .IsRequired();

    builder.Property(g => g.Percentage)
      .HasDefaultValue(0)
      .IsRequired();

    builder.ComplexProperty(g => g.GoalValue, gv =>
    {
      gv.Property(x => x.MinValue).IsRequired();
      gv.Property(x => x.MidValue).IsRequired();
      gv.Property(x => x.MaxValue).IsRequired();
      gv.Property(x => x.GoalValueType).HasConversion(p => p.Value, p => GoalValueType.FromValue(p)).IsRequired();
    });

    builder.Property(g => g.ActualValue);

    builder.HasOne<GoalSet>()
      .WithMany(gs => gs.Goals)
      .HasForeignKey(g => g.GoalSetId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasMany(g => g.GoalProgressHistory)
      .WithOne(gp => gp.Goal)
      .HasForeignKey(gp => gp.GoalId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(x => x.GoalProgress)
      .WithOne(x => x.CurrentGoal)
      .HasForeignKey<Goal>(x => x.ProgressId);

  

    builder.HasIndex(g => g.Title);
  }
}
