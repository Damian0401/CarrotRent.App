namespace Domain.Models;

public class Vehicle
{
    public Guid Id { get; set; }
    public string Description { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string Registration { get; set; } = default!;
    public string Vin { get; set; } = default!;
    public int Seats { get; set; }
    public int YearOfProduction { get; set; }
    public Guid VehicleStatusId { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid ModelId { get; set; } 
    public Guid PriceId { get; set; }
    public Guid FuelId { get; set; }

    public Fuel Fuel { get; set; } = default!;
    public Price Price { get; set; } = default!;
    public Model Model { get; set; } = default!;
    public Department Department { get; set; } = default!;
    public VehicleStatus VehicleStatus { get; set; } = default!;
    public ICollection<Rent> Rentings { get; set; } = default!;
}