namespace CarService.Application.DTOs;

public class CreateServiceOrderDto
{
    public Guid VehicleId { get; set; }
    public Guid MechanicId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal LaborCost { get; set; }
    public List<CreateServiceOrderPartDto> Parts { get; set; } = new();
}
