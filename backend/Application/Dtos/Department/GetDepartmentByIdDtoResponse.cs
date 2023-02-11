namespace Application.Dtos.Department;

public class GetDepartmentByIdDtoResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<VehicleForGetDepartmentByIdDtoResponse> Vehicles { get; set; } = default!;
    public string Manager { get; set; } = default!;
    public string Address { get; set; } = default!;
}

public class VehicleForGetDepartmentByIdDtoResponse
{
    public Guid Id { get; set; }
    public string Model { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public int YearOfProduction { get; set; }
    public string ImageUrl { get; set; } = default!;
}