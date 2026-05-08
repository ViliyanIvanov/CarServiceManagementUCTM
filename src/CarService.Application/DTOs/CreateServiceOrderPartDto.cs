namespace CarService.Application.DTOs;

public class CreateServiceOrderPartDto
{
    public Guid PartId { get; set; }
    public int Quantity { get; set; }
}
