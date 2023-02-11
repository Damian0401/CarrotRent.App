namespace Application.Dtos.Vehicle;

public class UpdateVehicleDtoRequest
{
    public string Description { get; set; } = default!;
    public double Price { get; set; }
    public int YearOfProduction { get; set; }
    public int Seats { get; set; }
    public string? ImageUrl { get; set; }
}