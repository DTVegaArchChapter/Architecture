using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GoalManager.Infrastructure.Identity;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
  : IdentityDbContext<ApplicationUser>(options);
