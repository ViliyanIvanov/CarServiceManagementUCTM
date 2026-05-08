using CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.Infrastructure.Persistence.Configurations;

public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        builder.ToTable("service_orders");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasColumnName("id");

        builder.Property(s => s.VehicleId).HasColumnName("vehicle_id");

        builder.Property(s => s.MechanicId).HasColumnName("mechanic_id");

        builder.Property(s => s.Description)
            .HasColumnName("description")
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(s => s.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.LaborCost)
            .HasColumnName("labor_cost")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(s => s.TotalCost)
            .HasColumnName("total_cost")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(s => s.CompletedAt)
            .HasColumnName("completed_at");
    }
}
