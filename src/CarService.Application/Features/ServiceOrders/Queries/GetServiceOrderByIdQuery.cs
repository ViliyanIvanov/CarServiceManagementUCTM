using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Exceptions;
using CarService.Application.Interfaces;
using CarService.Domain.Entities;
using MediatR;

namespace CarService.Application.Features.ServiceOrders.Queries;

public record GetServiceOrderByIdQuery(Guid Id) : IRequest<ServiceOrderDto>;

public class GetServiceOrderByIdQueryHandler : IRequestHandler<GetServiceOrderByIdQuery, ServiceOrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServiceOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceOrderDto> Handle(GetServiceOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.ServiceOrders.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceOrder), request.Id);

        var parts = await _unitOfWork.ServiceOrderParts.FindAsync(p => p.ServiceOrderId == order.Id, cancellationToken);
        order.ServiceOrderParts = parts.ToList();

        return _mapper.Map<ServiceOrderDto>(order);
    }
}
