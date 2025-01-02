using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GoalManager.Infrastructure.Identity;

public class IdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }
}
