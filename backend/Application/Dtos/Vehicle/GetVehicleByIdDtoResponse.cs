namespace Application.Dtos.Vehicle;

public class GetVehicleByIdDtoResponse
{
    public Guid Id { get; set; }
    public string Model { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public int YearOfProduction { get; set; }
    public string ImageUrl { get; set; } = default!;
    public string Fuel { get; set; } = default!;
    public int Seats { get; set; }
    public string Description { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string Department { get; set; } = default!;
    public Guid DepartmentId { get; set; }
    public double Price { get; set; }
}