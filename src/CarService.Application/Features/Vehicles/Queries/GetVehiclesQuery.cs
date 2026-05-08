using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Interfaces;
using MediatR;

namespace CarService.Application.Features.Vehicles.Queries;

public record GetVehiclesQuery() : IRequest<IReadOnlyList<VehicleDto>>;

public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, IReadOnlyList<VehicleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVehiclesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<VehicleDto>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _unitOfWork.Vehicles.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<VehicleDto>>(vehicles);
    }
}
