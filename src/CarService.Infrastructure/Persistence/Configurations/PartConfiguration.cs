using CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.Infrastructure.Persistence.Configurations;

public class PartConfiguration : IEntityTypeConfiguration<Part>
{
    public void Configure(EntityTypeBuilder<Part> builder)
    {
        builder.ToTable("parts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("id");

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.PartNumber)
            .HasColumnName("part_number")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.UnitPrice)
            .HasColumnName("unit_price")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(p => p.StockQuantity)
            .HasColumnName("stock_quantity")
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasIndex(p => p.PartNumber).IsUnique();
    }
}
