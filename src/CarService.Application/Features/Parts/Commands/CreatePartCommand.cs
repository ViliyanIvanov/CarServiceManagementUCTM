using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Interfaces;
using CarService.Domain.Entities;
using MediatR;

namespace CarService.Application.Features.Parts.Commands;

public record CreatePartCommand(CreatePartDto Part) : IRequest<PartDto>;

public class CreatePartCommandHandler : IRequestHandler<CreatePartCommand, PartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PartDto> Handle(CreatePartCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Part>(request.Part);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Parts.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PartDto>(entity);
    }
}
