using CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.Infrastructure.Persistence.Configurations;

public class MechanicConfiguration : IEntityTypeConfiguration<Mechanic>
{
    public void Configure(EntityTypeBuilder<Mechanic> builder)
    {
        builder.ToTable("mechanics");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id).HasColumnName("id");

        builder.Property(m => m.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.LastName)
            .HasColumnName("last_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Specialization)
            .HasColumnName("specialization")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Phone)
            .HasColumnName("phone")
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(m => m.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasMany(m => m.ServiceOrders)
            .WithOne(s => s.Mechanic)
            .HasForeignKey(s => s.MechanicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
