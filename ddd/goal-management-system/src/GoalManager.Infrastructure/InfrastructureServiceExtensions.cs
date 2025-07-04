﻿using GoalManager.Core.Organisation;
using GoalManager.Infrastructure.Data;
using GoalManager.Infrastructure.Identity;
using GoalManager.Infrastructure.Queries.GoalManagement;
using GoalManager.Infrastructure.Queries.Identity;
using GoalManager.Infrastructure.Queries.Notification;
using GoalManager.Infrastructure.Queries.Organisation;
using GoalManager.Infrastructure.Queries.PerformanceEvaluation;
using GoalManager.UseCases.GoalManagement;
using GoalManager.UseCases.Identity;
using GoalManager.UseCases.Notification;
using GoalManager.UseCases.Organisation;
using GoalManager.UseCases.PerformanceEvaluation;

namespace GoalManager.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    string? connectionString = config.GetConnectionString("DefaultConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
    services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString));

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
      .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
      .AddScoped<IIdentityQueryService, IdentityQueryService>()
      .AddScoped<IOrganisationQueryService, OrganisationQueryService>()
      .AddScoped<INotificationQueryService, NotificationQueryService>()
      .AddScoped<IOrganisationService, OrganisationService>()
      .AddScoped<IGoalManagementQueryService, GoalManagementQueryService>()
      .AddScoped<IPerformanceEvaluationQueryService, PerformanceEvaluationQueryService>();

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
