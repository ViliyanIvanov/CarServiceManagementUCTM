namespace CarService.Domain.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Vin { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Customer Customer { get; set; } = null!;
    public ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
}
