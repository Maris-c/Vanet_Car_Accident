using System.Reflection;
using AccidentMonitor.Application.Common.Interfaces;
using AccidentMonitor.Domain.Entities.Accident;
using AccidentMonitor.Domain.Entities.MapStuff.Polygons;
using AccidentMonitor.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccidentMonitor.Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    public DbSet<CitizenEntity> Citizens => Set<CitizenEntity>();
    public DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();
    public DbSet<AccidentEntity> Accidents => Set<AccidentEntity>();
    public DbSet<AccidentDetails> AccidentDetails => Set<AccidentDetails>();
    public DbSet<AccidentInvolved> AccidentInvolved => Set<AccidentInvolved>();
    public DbSet<PolygonCoordinate> PolygonCoordinates => Set<PolygonCoordinate>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
//// TODO: Refactor these to make it least access privileges to the database.
//public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
//{
//    public ApplicationDbContext CreateDbContext(string[] args)
//    {
//        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Web");
//        Console.WriteLine($"Base path: {basePath}");

//        var configuration = new ConfigurationBuilder()
//            .SetBasePath(basePath)
//            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//            .Build();

//        var migrationConnectionString = configuration.GetConnectionString("AccidentMonitorDB");
//        if (string.IsNullOrWhiteSpace(migrationConnectionString))
//        {
//            throw new InvalidOperationException("Connection string for migration actor not found.");
//        }

//        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
//        optionsBuilder.UseSqlServer(migrationConnectionString);

//        return new ApplicationDbContext(optionsBuilder.Options);
//    }
//}
