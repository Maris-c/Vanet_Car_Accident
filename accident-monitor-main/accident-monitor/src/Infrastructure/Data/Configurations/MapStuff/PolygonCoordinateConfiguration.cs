using AccidentMonitor.Domain.Entities.MapStuff.Polygons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccidentMonitor.Infrastructure.Data.Configurations.MapStuff
{
    public class PolygonCoordinateConfiguration : IEntityTypeConfiguration<PolygonCoordinate>
    {
        public void Configure(EntityTypeBuilder<PolygonCoordinate> builder)
        {
            builder.HasKey(v => v.Guid);
            builder.Property(v => v.Guid)
                .HasColumnName("Guid")
                .ValueGeneratedOnAdd();

            builder.HasOne(pc => pc.Accident)
                .WithMany(bp => bp.BlockedPolygonCoordinates)
                .HasForeignKey(pc => pc.AccidentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(pc => pc.Coordinate, coordinateBuilder =>
            {
                coordinateBuilder.Property(c => c.Longitude)
                    .HasColumnName("Longitude")
                    .IsRequired();
                coordinateBuilder.Property(c => c.Latitude)
                    .HasColumnName("Latitude")
                    .IsRequired();
            });
            // TODO: Fix the check constraint, it does not appear in the database 
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Coordinate_Latitude_Range", "[Latitude] >= -90 AND [Latitude] <= 90");
                t.HasCheckConstraint("CK_Coordinate_Longitude_Range", "[Longitude] >= -180 AND [Longitude] <= 180");
            });

            builder.Property(pc => pc.AccidentId).IsRequired();
        }
    }
}
