using AccidentMonitor.Application.Common.Interfaces;
using AccidentMonitor.Domain.Constants;
using AccidentMonitor.Infrastructure.Data;
using AccidentMonitor.Infrastructure.Data.Interceptors;
using AccidentMonitor.Infrastructure.Identity;
using AccidentMonitor.Infrastructure.MQTT;
using AccidentMonitor.Infrastructure.ORS;
using AccidentMonitor.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AccidentMonitor.Infrastructure;
public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionStrings = builder.Configuration.GetConnectionString("AccidentMonitorDB");

        Guard.Against.Null(connectionStrings, message: "Connection string 'AccidentMonitorDB' not found.");

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, AccidentEntityInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionStrings);
            options.ConfigureWarnings(warnings =>
               warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        builder.EnrichSqlServerDbContext<ApplicationDbContext>();

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddScoped<ApplicationDbContextInitializer>();

        builder.Services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        builder.Services.AddAuthorizationBuilder();

        builder.Services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator));

        // Repositories
        builder.Services.AddTransient<IAccidentRepository, AccidentRepository>();
        builder.Services.AddTransient<IBlockedPolygonCoordRepository, BlockedPolygonCoordinateRepository>();

        // MQTT Service
        builder.Services.Configure<MqttConnectionConfiguration>
            (builder.Configuration.GetSection("MqttConnectionConfig"));
        builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<MqttConnectionConfiguration>>().Value);
        builder.Services.AddSingleton<IMqttService, MqttClientService>();
        builder.Services.AddScoped<MqttClientContextInitializer>();

        // ORService 
        builder.Services.AddSingleton<IORService, ORService>();
        var orsConnectionString = builder.Configuration.GetSection("ORSUri");
        builder.Services.Configure<ORSConfiguration>(builder.Configuration.GetSection("ORS"));
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<ORSConfiguration>>().Value);
    }
}
