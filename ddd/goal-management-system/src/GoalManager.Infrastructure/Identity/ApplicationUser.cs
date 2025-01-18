using Microsoft.AspNetCore.Identity;

namespace GoalManager.Infrastructure.Identity;

// Add profile data for application users by adding properties to the ApplicationUser class
#pragma warning disable S2094
public class ApplicationUser : IdentityUser<int>;
#pragma warning restore S2094

