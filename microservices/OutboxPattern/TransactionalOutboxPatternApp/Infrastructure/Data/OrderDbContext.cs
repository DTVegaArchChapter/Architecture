namespace TransactionalOutboxPatternApp.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

using TransactionalOutboxPatternApp.Infrastructure.Entity;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    public DbSet<EventLogEntry> EventLogEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(
            builder =>
                {
                    builder.ToTable("Order");
                    builder.HasKey(e => e.Id);
                    builder.Property(e => e.Id).UseIdentityColumn();
                    builder.Property(e => e.CreateDate).IsRequired();
                });

        modelBuilder.Entity<EventLogEntry>(
            builder =>
                {
                    builder.ToTable("EventLogEntry");
                    builder.HasKey(e => e.EventId);
                    builder.Property(e => e.EventId).IsRequired();
                    builder.Property(e => e.TransactionId).IsRequired();
                    builder.HasIndex(e => e.TransactionId).IsUnique();
                    builder.Property(e => e.Content).IsRequired();
                    builder.Property(e => e.CreateDate).IsRequired();
                    builder.Property(e => e.State).IsRequired();
                    builder.Property(e => e.TimesSent).IsRequired();
                    builder.Property(e => e.EventTypeName).IsRequired();
                });
    }
}