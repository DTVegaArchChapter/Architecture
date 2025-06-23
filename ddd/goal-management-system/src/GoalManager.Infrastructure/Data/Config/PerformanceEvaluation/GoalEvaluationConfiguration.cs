using GoalManager.Core.PerformanceEvaluation;

namespace GoalManager.Infrastructure.Data.Config.PerformanceEvaluation;

internal sealed class GoalEvaluationConfiguration : IEntityTypeConfiguration<GoalEvaluation>
{
  public void Configure(EntityTypeBuilder<GoalEvaluation> builder)
  {
    builder.HasKey(g => g.Id);

    builder.Property(g => g.GoalTitle)
      .IsRequired()
      .HasMaxLength(200);

    builder.Property(g => g.Point);

    builder.Property(g => g.Percentage)
      .HasDefaultValue(0)
      .IsRequired();

    builder.ComplexProperty(g => g.GoalValue, gv =>
    {
      gv.Property(x => x.MinValue).IsRequired();
      gv.Property(x => x.MidValue).IsRequired();
      gv.Property(x => x.MaxValue).IsRequired();
    });

    builder.Property(g => g.ActualValue);

    builder.HasOne<GoalSetEvaluation>()
      .WithMany(gs => gs.GoalEvaluations)
      .HasForeignKey(g => g.GoalSetEvaluationId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasIndex(g => g.GoalTitle);
  }
}
