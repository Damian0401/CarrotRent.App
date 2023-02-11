namespace Domain.Models;

public class Price
{
    public Guid Id { get; set; }
    public double PricePerDay { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = default!;
}