using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Interfaces;
using CarService.Domain.Entities;
using MediatR;

namespace CarService.Application.Features.Mechanics.Commands;

public record CreateMechanicCommand(CreateMechanicDto Mechanic) : IRequest<MechanicDto>;

public class CreateMechanicCommandHandler : IRequestHandler<CreateMechanicCommand, MechanicDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMechanicCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MechanicDto> Handle(CreateMechanicCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Mechanic>(request.Mechanic);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Mechanics.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MechanicDto>(entity);
    }
}
