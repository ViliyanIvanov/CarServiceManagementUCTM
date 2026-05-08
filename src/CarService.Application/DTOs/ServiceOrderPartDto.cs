namespace CarService.Application.DTOs;

public class ServiceOrderPartDto
{
    public Guid PartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
