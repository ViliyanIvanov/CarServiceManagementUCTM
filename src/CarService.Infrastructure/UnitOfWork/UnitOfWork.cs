using CarService.Application.Interfaces;
using CarService.Domain.Entities;
using CarService.Infrastructure.Persistence;
using CarService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarService.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<Customer>? _customers;
    private IRepository<Vehicle>? _vehicles;
    private IRepository<Mechanic>? _mechanics;
    private IRepository<Part>? _parts;
    private IRepository<ServiceOrder>? _serviceOrders;
    private IRepository<ServiceOrderPart>? _serviceOrderParts;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<Customer> Customers => _customers ??= new Repository<Customer>(_context);
    public IRepository<Vehicle> Vehicles => _vehicles ??= new Repository<Vehicle>(_context);
    public IRepository<Mechanic> Mechanics => _mechanics ??= new Repository<Mechanic>(_context);
    public IRepository<Part> Parts => _parts ??= new Repository<Part>(_context);
    public IRepository<ServiceOrder> ServiceOrders => _serviceOrders ??= new Repository<ServiceOrder>(_context);
    public IRepository<ServiceOrderPart> ServiceOrderParts => _serviceOrderParts ??= new Repository<ServiceOrderPart>(_context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction ??= await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null) return;

        try
        {
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null) return;

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
