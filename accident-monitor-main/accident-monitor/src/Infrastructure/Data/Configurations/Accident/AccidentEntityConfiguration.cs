using AccidentMonitor.Domain.Entities.Accident;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccidentMonitor.Infrastructure.Data.Configurations.Accident;
public class AccidentEntityConfiguration : IEntityTypeConfiguration<AccidentEntity>
{
    public void Configure(EntityTypeBuilder<AccidentEntity> builder)
    {
        builder.HasKey(a => a.Guid);
        builder.Property(v => v.Guid)
            .HasColumnName("Guid")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Timestamp).IsRequired();
        builder.Property(a => a.Longitude).IsRequired();
        builder.Property(a => a.Latitude).IsRequired();

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Coordinate_Latitude_Range", "[Latitude] >= -90 AND [Latitude] <= 90");
            t.HasCheckConstraint("CK_Coordinate_Longitude_Range", "[Longitude] >= -180 AND [Longitude] <= 180");
        });

        builder.HasMany(a => a.AccidentInvolved)
            .WithOne(av => av.Accident)
            .HasForeignKey(av => av.AccidentId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
