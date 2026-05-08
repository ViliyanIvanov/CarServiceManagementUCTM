using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Exceptions;
using CarService.Application.Interfaces;
using CarService.Domain.Entities;
using MediatR;

namespace CarService.Application.Features.Vehicles.Commands;

public record UpdateVehicleCommand(Guid Id, UpdateVehicleDto Vehicle) : IRequest<bool>;

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateVehicleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Vehicles.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Vehicle), request.Id);

        _mapper.Map(request.Vehicle, entity);
        _unitOfWork.Vehicles.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
