namespace TaskManagementApi.Infrastructure
{
    using Microsoft.EntityFrameworkCore;

    public class TaskEntity
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public bool Completed { get; set; }
    }

    public class TaskDbContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; }

        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TaskEntity>().HasKey(m => m.Id);
            builder.Entity<TaskEntity>().Property(m => m.Title).HasMaxLength(300).IsRequired();

            base.OnModelCreating(builder);
        }
    }
}
