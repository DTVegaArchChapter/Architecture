using GoalManager.Core.Interfaces;
using GoalManager.Core.Organisation;
using GoalManager.Core.Services;
using GoalManager.Infrastructure.Data;
using GoalManager.Infrastructure.Data.Queries;
using GoalManager.Infrastructure.Data.Queries.Organisation;
using GoalManager.Infrastructure.Identity;
using GoalManager.UseCases.Contributors.List;
using GoalManager.UseCases.Organisation.List;

namespace GoalManager.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    string? connectionString = config.GetConnectionString("SqliteConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

    var identityConnectionString = config.GetConnectionString("IdentityDbContextConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<IdentityDbContext>(options => options.UseSqlite(identityConnectionString));

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
      .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
      .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
      .AddScoped<IDeleteContributorService, DeleteContributorService>()
      .AddScoped<IOrganisationQueryService, OrganisationQueryService>()
      .AddScoped<IOrganisationService, OrganisationService>();


    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
