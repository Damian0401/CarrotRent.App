using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Vehicle;

public class GetFilteredVehiclesDtoRequest
{
    public Guid? BrandId { get; set; } = default!;
    public Guid? FuelId { get; set; } = default!;
    public Guid? DepartmentId { get; set; } = default!;
    public Guid? ModelId { get; set; } = default!;
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public int? Seats { get; set; }

    public static ValueTask<GetFilteredVehiclesDtoRequest?> BindAsync(HttpContext context)
        => ValueTask.FromResult<GetFilteredVehiclesDtoRequest?>(
            new GetFilteredVehiclesDtoRequest
            {
                BrandId = Guid.TryParse(context.Request.Query["brandId"], out var brandId) ? brandId : null,
                FuelId = Guid.TryParse(context.Request.Query["fuelId"], out var fuelId) ? fuelId : null,
                DepartmentId = Guid.TryParse(context.Request.Query["departmentId"], out var departmentId) ? departmentId : null,
                ModelId = Guid.TryParse(context.Request.Query["modelId"], out var modelId) ? modelId : null,
                MinPrice = double.TryParse(context.Request.Query["minPrice"], out var minPrice) ? minPrice : null,
                MaxPrice = double.TryParse(context.Request.Query["maxPrice"], out var maxPrice) ? maxPrice : null,
                Seats = int.TryParse(context.Request.Query["seats"], out var seats) ? seats : null,
            }
        );
}