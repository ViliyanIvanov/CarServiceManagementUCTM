using CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id).HasColumnName("id");

        builder.Property(v => v.CustomerId).HasColumnName("customer_id");

        builder.Property(v => v.Make)
            .HasColumnName("make")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(v => v.Model)
            .HasColumnName("model")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(v => v.Year)
            .HasColumnName("year")
            .IsRequired();

        builder.Property(v => v.LicensePlate)
            .HasColumnName("license_plate")
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(v => v.Vin)
            .HasColumnName("vin")
            .IsRequired()
            .HasMaxLength(17);

        builder.Property(v => v.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasIndex(v => v.Vin).IsUnique();
        builder.HasIndex(v => v.LicensePlate).IsUnique();

        builder.HasMany(v => v.ServiceOrders)
            .WithOne(s => s.Vehicle)
            .HasForeignKey(s => s.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
