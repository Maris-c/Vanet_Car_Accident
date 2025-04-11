using AccidentMonitor.Domain.Entities.Accident;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccidentMonitor.Infrastructure.Data.Configurations.Accident;
public class CitizenEntityConfiguration : IEntityTypeConfiguration<CitizenEntity>
{
    public void Configure(EntityTypeBuilder<CitizenEntity> builder)
    {
        builder.HasKey(c => c.Guid);
        builder.Property(c => c.Guid)
            .HasColumnName("Guid")
            .ValueGeneratedOnAdd();

        builder.HasIndex(c => c.CitizenIdentityNumber).IsUnique();
        builder.HasIndex(c => c.VerifiedPhoneNumber).IsUnique();

        builder.Property(c => c.CitizenIdentityNumber)
            .HasConversion<string>()
            .HasMaxLength(12)
            .IsRequired();
        builder.Property(c => c.FullName)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(c => c.DateOfBirth)
            .IsRequired();
        builder.Property(c => c.IsMale)
            .IsRequired();
        builder.Property(c => c.Nationality)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(c => c.PlaceOfOrigin)
            .IsRequired();
        builder.Property(c => c.PlaceOfResidence)
            .IsRequired();

        builder.HasMany(c => c.Vehicles)
            .WithOne(cv => cv.Owner)
            .HasForeignKey(cv => cv.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.AccidentsInvolved)
           .WithOne(av => av.DriverInvolved)
           .HasForeignKey(av => av.DriverCitizenId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
