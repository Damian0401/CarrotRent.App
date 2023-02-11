namespace Domain.Models;

public class VehicleStatus
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<Vehicle> Vehicles { get; set; } = default!;
}