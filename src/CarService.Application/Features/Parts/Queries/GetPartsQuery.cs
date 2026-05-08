using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Interfaces;
using MediatR;

namespace CarService.Application.Features.Parts.Queries;

public record GetPartsQuery() : IRequest<IReadOnlyList<PartDto>>;

public class GetPartsQueryHandler : IRequestHandler<GetPartsQuery, IReadOnlyList<PartDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPartsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PartDto>> Handle(GetPartsQuery request, CancellationToken cancellationToken)
    {
        var parts = await _unitOfWork.Parts.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<PartDto>>(parts);
    }
}
