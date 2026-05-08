using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Interfaces;
using MediatR;

namespace CarService.Application.Features.Mechanics.Queries;

public record GetMechanicsQuery() : IRequest<IReadOnlyList<MechanicDto>>;

public class GetMechanicsQueryHandler : IRequestHandler<GetMechanicsQuery, IReadOnlyList<MechanicDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMechanicsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MechanicDto>> Handle(GetMechanicsQuery request, CancellationToken cancellationToken)
    {
        var mechanics = await _unitOfWork.Mechanics.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<MechanicDto>>(mechanics);
    }
}
