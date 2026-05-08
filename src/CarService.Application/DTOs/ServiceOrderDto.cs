namespace CarService.Application.DTOs;

public class ServiceOrderDto
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Guid MechanicId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal LaborCost { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<ServiceOrderPartDto> Parts { get; set; } = new();
}
