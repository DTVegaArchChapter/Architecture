using GoalManager.Infrastructure.Identity;
using GoalManager.Web.Configurations;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.MinimumSameSitePolicy = SameSiteMode.Strict;
});
builder.Services.ConfigureApplicationCookie(options =>
{
  options.Cookie.HttpOnly = true;
  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
  options.LoginPath = "/Identity/Account/Login";
  options.LogoutPath = "/Identity/Account/Logout";
  options.Cookie = new CookieBuilder
                   {
                     IsEssential = true // required for auth to work without explicit user consent; adjust to suit your privacy policy
                   };
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
  options =>
  {
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
  });

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
  .AddEntityFrameworkStores<IdentityDbContext>();

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

builder.AddLoggerConfigs();

var appLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<Program>();

builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);
builder.Services.AddServiceConfigs(appLogger, builder);

builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                  o.ShortSchemaNames = true;
                });
builder.Services.AddRazorPages(
  o =>
  {
    o.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
    o.Conventions.AllowAnonymousToAreaFolder("Identity", "/Account");
  });

builder.Services.AddAuthorization(options =>
{
  options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
});

var app = builder.Build();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

await app.UseAppMiddlewareAndSeedDatabase();
app.MapRazorPages();

app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
