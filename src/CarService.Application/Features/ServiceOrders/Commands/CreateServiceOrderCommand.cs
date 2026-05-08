using AutoMapper;
using CarService.Application.DTOs;
using CarService.Application.Exceptions;
using CarService.Application.Interfaces;
using CarService.Domain.Entities;
using MediatR;

namespace CarService.Application.Features.ServiceOrders.Commands;

public record CreateServiceOrderCommand(CreateServiceOrderDto Order) : IRequest<ServiceOrderDto>;

public class CreateServiceOrderCommandHandler : IRequestHandler<CreateServiceOrderCommand, ServiceOrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateServiceOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceOrderDto> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Order;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var order = new ServiceOrder
            {
                Id = Guid.NewGuid(),
                VehicleId = dto.VehicleId,
                MechanicId = dto.MechanicId,
                Description = dto.Description,
                Status = "Pending",
                LaborCost = dto.LaborCost,
                CreatedAt = DateTime.UtcNow
            };

            decimal partsTotal = 0m;

            foreach (var item in dto.Parts)
            {
                var part = await _unitOfWork.Parts.GetByIdAsync(item.PartId, cancellationToken)
                    ?? throw new NotFoundException(nameof(Part), item.PartId);

                if (part.StockQuantity < item.Quantity)
                {
                    throw new BusinessException(
                        $"Insufficient stock for part \"{part.Name}\". Available: {part.StockQuantity}, requested: {item.Quantity}.");
                }

                part.StockQuantity -= item.Quantity;
                _unitOfWork.Parts.Update(part);

                order.ServiceOrderParts.Add(new ServiceOrderPart
                {
                    ServiceOrderId = order.Id,
                    PartId = part.Id,
                    Quantity = item.Quantity,
                    UnitPrice = part.UnitPrice
                });

                partsTotal += part.UnitPrice * item.Quantity;
            }

            order.TotalCost = order.LaborCost + partsTotal;

            await _unitOfWork.ServiceOrders.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return _mapper.Map<ServiceOrderDto>(order);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
