using CarService.Domain.Entities;

namespace CarService.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Customer> Customers { get; }
    IRepository<Vehicle> Vehicles { get; }
    IRepository<Mechanic> Mechanics { get; }
    IRepository<Part> Parts { get; }
    IRepository<ServiceOrder> ServiceOrders { get; }
    IRepository<ServiceOrderPart> ServiceOrderParts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
