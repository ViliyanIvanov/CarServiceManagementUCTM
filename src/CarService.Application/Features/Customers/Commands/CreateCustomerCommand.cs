using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Interfaces;
using CarService.Domain.Entities;
using MediatR;

namespace CarService.Application.Features.Customers.Commands;

public record CreateCustomerCommand(CreateCustomerDto Customer) : IRequest<CustomerDto>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Customer>(request.Customer);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Customers.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CustomerDto>(entity);
    }
}
