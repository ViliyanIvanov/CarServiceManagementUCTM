using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Interfaces;
using MediatR;

namespace CarService.Application.Features.Customers.Queries;

public record GetCustomersQuery() : IRequest<IReadOnlyList<CustomerDto>>;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IReadOnlyList<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _unitOfWork.Customers.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<CustomerDto>>(customers);
    }
}
