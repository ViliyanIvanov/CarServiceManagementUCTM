using CarService.Application.Exceptions;
using CarService.Application.Interfaces;
using CarService.Domain.Entities;
using MediatR;

namespace CarService.Application.Features.Vehicles.Commands;

public record DeleteVehicleCommand(Guid Id) : IRequest<bool>;

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Vehicles.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Vehicle), request.Id);

        _unitOfWork.Vehicles.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
