namespace Domain.Models;

public class Fuel
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;

    public ICollection<Vehicle> Vehicles { get; set; } = default!;
}