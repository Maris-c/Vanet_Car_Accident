using AccidentMonitor.Domain.Entities.Accident;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccidentMonitor.Infrastructure.Data.Configurations.Accident;
public class AccidentInvolvedConfiguration : IEntityTypeConfiguration<AccidentInvolved>
{
    public void Configure(EntityTypeBuilder<AccidentInvolved> builder)
    {
        builder.HasKey(ai => new { ai.AccidentId, ai.VehicleId, ai.DriverCitizenId });

        builder.Property(v => v.Guid)
            .HasColumnName("Guid")
            .ValueGeneratedOnAdd();

        builder.HasOne(ai => ai.DriverInvolved)
            .WithMany(d => d.AccidentsInvolved)
            .HasForeignKey(ai => ai.DriverCitizenId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ai => ai.VehicleInvolved)
            .WithMany(v => v.AccidentInvolved)
            .HasForeignKey(ai => ai.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
