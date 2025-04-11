using AccidentMonitor.Domain.Entities.Accident;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccidentMonitor.Infrastructure.Data.Configurations.Accident;
public class VehicleEntityConfiguration : IEntityTypeConfiguration<VehicleEntity>
{
    public void Configure(EntityTypeBuilder<VehicleEntity> builder)
    {
        builder.HasKey(v => v.Guid);
        builder.Property(v => v.Guid)
            .HasColumnName("Guid")
            .ValueGeneratedOnAdd();

        builder.HasIndex(v => v.LicensePlate).IsUnique();
        builder.HasIndex(v => v.RegistrationCertificateNumber).IsUnique();

        builder.Property(a => a.VehicleOwnerName).IsRequired();
        builder.Property(a => a.Type).IsRequired();
        builder.Property(a => a.RegistrationCertificateNumber).IsRequired();
        builder.Property(a => a.Model).IsRequired();
        builder.Property(a => a.LicensePlate).IsRequired();
        builder.Property(a => a.EngineNumber).IsRequired();
        builder.Property(a => a.ChassisNumber).IsRequired();
        builder.Property(a => a.Brand).IsRequired();
    }
}
