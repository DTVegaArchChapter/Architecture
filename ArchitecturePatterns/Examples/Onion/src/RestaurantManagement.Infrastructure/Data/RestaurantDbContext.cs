using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Entities;

namespace RestaurantManagement.Infrastructure.Data;

public class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : DbContext(options)
{
    public DbSet<Table> Tables => Set<Table>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table configuration
        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.TableNumber).IsRequired();
            entity.Property(e => e.Capacity).IsRequired();
            entity.Property(e => e.Status).IsRequired();
        });

        // MenuItem configuration
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Price).IsRequired().HasPrecision(18, 2);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.OrderNumber).HasMaxLength(25).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Table)
                .WithMany()
                .HasForeignKey(e => e.TableId)
                .IsRequired();

            entity.HasMany(e => e.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // OrderItem configuration
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.SpecialInstructions).HasMaxLength(250);
            
            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.MenuItem)
                .WithMany()
                .HasForeignKey(e => e.MenuItemId)
                .IsRequired();
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Tables
        modelBuilder.Entity<Table>().HasData(
            new { Id = 1, TableNumber = 1, Capacity = 4, Status = TableStatus.Available, ReservedAt = (DateTime?)null },
            new { Id = 2, TableNumber = 2, Capacity = 2, Status = TableStatus.Available, ReservedAt = (DateTime?)null },
            new { Id = 3, TableNumber = 3, Capacity = 6, Status = TableStatus.Available, ReservedAt = (DateTime?)null },
            new { Id = 4, TableNumber = 4, Capacity = 4, Status = TableStatus.Available, ReservedAt = (DateTime?)null },
            new { Id = 5, TableNumber = 5, Capacity = 8, Status = TableStatus.Available, ReservedAt = (DateTime?)null }
        );

        // Seed MenuItems
        modelBuilder.Entity<MenuItem>().HasData(
            new { Id = 1, Name = "Margherita Pizza", Category = "Pizza", Price = 12.99m, Description = "Classic tomato and mozzarella", IsAvailable = true },
            new { Id = 2, Name = "Pepperoni Pizza", Category = "Pizza", Price = 14.99m, Description = "Pepperoni and mozzarella", IsAvailable = true },
            new { Id = 3, Name = "Caesar Salad", Category = "Salads", Price = 8.99m, Description = "Fresh romaine with caesar dressing", IsAvailable = true },
            new { Id = 4, Name = "Grilled Chicken", Category = "Main Course", Price = 18.99m, Description = "Grilled chicken breast with vegetables", IsAvailable = true },
            new { Id = 5, Name = "Pasta Carbonara", Category = "Pasta", Price = 15.99m, Description = "Creamy pasta with bacon", IsAvailable = true },
            new { Id = 6, Name = "Chocolate Cake", Category = "Desserts", Price = 6.99m, Description = "Rich chocolate cake", IsAvailable = true },
            new { Id = 7, Name = "Coffee", Category = "Beverages", Price = 2.99m, Description = "Fresh brewed coffee", IsAvailable = true },
            new { Id = 8, Name = "Orange Juice", Category = "Beverages", Price = 3.99m, Description = "Fresh squeezed orange juice", IsAvailable = true }
        );
    }
}