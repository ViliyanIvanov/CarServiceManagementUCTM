namespace CarService.Application.DTOs;

public class UpdateVehicleDto
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Vin { get; set; } = string.Empty;
}
