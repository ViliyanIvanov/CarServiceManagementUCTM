using CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.Infrastructure.Persistence.Configurations;

public class ServiceOrderPartConfiguration : IEntityTypeConfiguration<ServiceOrderPart>
{
    public void Configure(EntityTypeBuilder<ServiceOrderPart> builder)
    {
        builder.ToTable("service_order_parts");

        builder.HasKey(sp => new { sp.ServiceOrderId, sp.PartId });

        builder.Property(sp => sp.ServiceOrderId).HasColumnName("service_order_id");

        builder.Property(sp => sp.PartId).HasColumnName("part_id");

        builder.Property(sp => sp.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(sp => sp.UnitPrice)
            .HasColumnName("unit_price")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.HasOne(sp => sp.ServiceOrder)
            .WithMany(s => s.ServiceOrderParts)
            .HasForeignKey(sp => sp.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.Part)
            .WithMany(p => p.ServiceOrderParts)
            .HasForeignKey(sp => sp.PartId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
