namespace CarService.Application.DTOs;

public class VehicleDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Vin { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
