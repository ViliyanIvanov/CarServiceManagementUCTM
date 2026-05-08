using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Interfaces;
using CarService.Domain.Entities;
using MediatR;

namespace CarService.Application.Features.Vehicles.Commands;

public record CreateVehicleCommand(CreateVehicleDto Vehicle) : IRequest<VehicleDto>;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateVehicleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleDto> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Vehicle>(request.Vehicle);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Vehicles.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<VehicleDto>(entity);
    }
}
