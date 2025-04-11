using AccidentMonitor.Domain.Constants;
using AccidentMonitor.Domain.Entities.Accident;
using AccidentMonitor.Domain.Enums;
using AccidentMonitor.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AccidentMonitor.Infrastructure.Data;
public static class InitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

        await initializer.InitializeAsync();

        //await initializer.SeedAsync();
    }
}

public class ApplicationDbContextInitializer(
    ILogger<ApplicationDbContextInitializer> logger,
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
{
    public async Task InitializeAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
        }

        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await userManager.AddToRolesAsync(administrator, [administratorRole.Name]);
            }
        }
        if (!context.Citizens.Any())
        {
            context.Citizens.AddRange(
                new CitizenEntity(
                    "123456789012",
                    "Nguyễn Văn A",
                    new DateOnly(1990, 5, 20),
                    true,
                    "Vietnam",
                    "Hà Nội",
                    "Hồ Chí Minh",
                    "0987654321"
                ),
                new CitizenEntity(
                    "234567890123",
                    "Trần Thị B",
                    new DateOnly(1995, 8, 15),
                    false,
                    "Vietnam",
                    "Đà Nẵng",
                    "Hà Nội",
                    "0976543210"
                ),
                new CitizenEntity(
                    "345678901234",
                    "Lê Văn C",
                    new DateOnly(1985, 12, 10),
                    true,
                    "Vietnam",
                    "Hải Phòng",
                    "Hải Phòng",
                    "0965432109"
                )
            );

            await context.SaveChangesAsync();
        }

        if (!context.Vehicles.Any())
        {
            // Lấy list Citizens vừa tạo
            var citizens = context.Citizens.ToList();

            var vehicles = new[]
            {
            new VehicleEntity(
                registrationCertificateNumber: "RCN-0001",
                licensePlate: "30A-12345",
                engineNumber: "ENG-1001",
                chassisNumber: "CHS-1001",
                type: "Car",
                brand: "Toyota",
                model: "Vios",
                vehicleOwnerName: citizens[0].FullName,
                address: "Hà Nội",
                ownerId: citizens[0].Guid
            ),
            new VehicleEntity(
                registrationCertificateNumber: "RCN-0002",
                licensePlate: "43B-54321",
                engineNumber: "ENG-2002",
                chassisNumber: "CHS-2002",
                type: "Motorbike",
                brand: "Honda",
                model: "Wave",
                vehicleOwnerName: citizens[1].FullName,
                address: "Đà Nẵng",
                ownerId: citizens[1].Guid
            ),
            new VehicleEntity(
                registrationCertificateNumber: "RCN-0003",
                licensePlate: "15C-67890",
                engineNumber: "ENG-3003",
                chassisNumber: "CHS-3003",
                type: "Truck",
                brand: "Isuzu",
                model: "Forward",
                vehicleOwnerName: citizens[2].FullName,
                address: "Hải Phòng",
                ownerId : citizens[2].Guid
            )
        };

            context.Vehicles.AddRange(vehicles);
            await context.SaveChangesAsync();
        }


        if (!context.Accidents.Any())
        {
            var accident = new AccidentEntity(
                DateTime.Now,
                106.66069019038777F,
                10.801754663322315F,
                true,
                AccidentSeverity.Medium,
                AccidentResolvedStatus.Unresolved
            );

            context.Accidents.Add(accident);

            var firstVehicle = context.Vehicles.FirstOrDefault();
            var firstCitizen = context.Citizens.FirstOrDefault();

            if (firstVehicle != null && firstCitizen != null)
            {
                accident.AccidentInvolved.Add(new AccidentInvolved
                {
                    AccidentId = accident.Guid,
                    VehicleId = firstVehicle.Guid,
                    DriverCitizenId = firstCitizen.Guid,
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
