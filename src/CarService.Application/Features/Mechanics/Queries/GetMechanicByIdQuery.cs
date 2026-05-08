using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Exceptions;
using CarService.Application.Interfaces;
using CarService.Domain.Entities;
using MediatR;

namespace CarService.Application.Features.Mechanics.Queries;

public record GetMechanicByIdQuery(Guid Id) : IRequest<MechanicDto>;

public class GetMechanicByIdQueryHandler : IRequestHandler<GetMechanicByIdQuery, MechanicDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMechanicByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MechanicDto> Handle(GetMechanicByIdQuery request, CancellationToken cancellationToken)
    {
        var mechanic = await _unitOfWork.Mechanics.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Mechanic), request.Id);

        return _mapper.Map<MechanicDto>(mechanic);
    }
}
