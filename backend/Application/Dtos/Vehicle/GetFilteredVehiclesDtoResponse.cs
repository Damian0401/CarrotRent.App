namespace Application.Dtos.Vehicle;

public class GetFilteredVehiclesDtoResponse
{
    public List<VehicleForGetFilteredVehiclesDtoResponse> Vehicles { get; set; } = default!;
}

public class VehicleForGetFilteredVehiclesDtoResponse
{
    public Guid Id { get; set; }
    public string Model { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public int YearOfProduction { get; set; }
    public double Price { get; set; }
    public string ImageUrl { get; set; } = default!;
}