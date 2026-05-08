namespace CarService.Domain.Entities;

public class ServiceOrderPart
{
    public Guid ServiceOrderId { get; set; }
    public Guid PartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public ServiceOrder ServiceOrder { get; set; } = null!;
    public Part Part { get; set; } = null!;
}
