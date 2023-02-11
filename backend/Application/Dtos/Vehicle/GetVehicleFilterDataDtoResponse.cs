namespace Application.Dtos.Vehicle;

public class GetVehicleFilterDataDtoResponse
{
    public List<FuelForGetVehicleFilterDataDtoResponse> Fuels { get; set; } = default!;
    public List<BrandForGetVehicleFilterDataDtoResponse> Brands { get; set; } = default!;
    public List<DepartmentForGetVehicleFilterDataDtoResponse> Departments { get; set; } = default!;

}

public class FuelForGetVehicleFilterDataDtoResponse
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
}

public class BrandForGetVehicleFilterDataDtoResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<ModelForGetVehicleFilterDataDtoResponse> Models { get; set; } = default!;
}

public class ModelForGetVehicleFilterDataDtoResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

public class DepartmentForGetVehicleFilterDataDtoResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
