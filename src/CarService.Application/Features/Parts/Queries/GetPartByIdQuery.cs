using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Exceptions;
using CarService.Application.Interfaces;
using CarService.Domain.Entities;
using MediatR;

namespace CarService.Application.Features.Parts.Queries;

public record GetPartByIdQuery(Guid Id) : IRequest<PartDto>;

public class GetPartByIdQueryHandler : IRequestHandler<GetPartByIdQuery, PartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPartByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PartDto> Handle(GetPartByIdQuery request, CancellationToken cancellationToken)
    {
        var part = await _unitOfWork.Parts.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Part), request.Id);

        return _mapper.Map<PartDto>(part);
    }
}
