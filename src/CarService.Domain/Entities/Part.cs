namespace CarService.Domain.Entities;

public class Part
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<ServiceOrderPart> ServiceOrderParts { get; set; } = new List<ServiceOrderPart>();
}
