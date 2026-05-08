using CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarService.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Mechanic> Mechanics => Set<Mechanic>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<ServiceOrder> ServiceOrders => Set<ServiceOrder>();
    public DbSet<ServiceOrderPart> ServiceOrderParts => Set<ServiceOrderPart>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        StampCreatedAt();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        StampCreatedAt();
        return base.SaveChanges();
    }

    private void StampCreatedAt()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State != EntityState.Added) continue;

            var property = entry.Metadata.FindProperty("CreatedAt");
            if (property is null || property.ClrType != typeof(DateTime)) continue;

            var current = entry.Property("CreatedAt").CurrentValue;
            if (current is DateTime dt && dt == default)
            {
                entry.Property("CreatedAt").CurrentValue = now;
            }
        }
    }
}
