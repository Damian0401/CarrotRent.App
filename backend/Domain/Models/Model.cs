namespace Domain.Models;

public class Model
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid BrandId { get; set; }

    public Brand Brand { get; set; } = default!;
    public ICollection<Vehicle> Vehicles { get; set; } = default!;
}