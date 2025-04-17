using GoalManager.Core.GoalManagement;
using GoalManager.Core.Notification;
using GoalManager.Core.Organisation;

using SmartEnum.EFCore;

namespace GoalManager.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options,
  IDomainEventDispatcher? dispatcher) : DbContext(options)
{
  private readonly IDomainEventDispatcher? _dispatcher = dispatcher;

  public DbSet<Organisation> Organisation => Set<Organisation>();

  public DbSet<Team> Team => Set<Team>();

  public DbSet<TeamMember> TeamMember => Set<TeamMember>();

  public DbSet<NotificationItem> NotificationItem => Set<NotificationItem>();
  public DbSet<GoalSet> GoalSet => Set<GoalSet>();
  public DbSet<Goal> Goal => Set<Goal>();
  public DbSet<GoalProgress> GoalProgress => Set<GoalProgress>();
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.ConfigureSmartEnum();
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    var entitiesWithEvents = ChangeTracker.Entries<HasDomainEventsBase>()
      .Select(e => e.Entity)
      .Where(e => e.DomainEvents.Any())
      .ToArray();

    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;

    // dispatch events only if save was successful
    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();
}
