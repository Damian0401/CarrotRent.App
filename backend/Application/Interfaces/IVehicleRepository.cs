using Application.Dtos.Vehicle;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IVehicleRepository
    {
        List<Fuel> GetAllFuels();
        List<Brand> GetAllBrands();
        List<Department> GetAllDepartments();
        Fuel? GetFuelById(Guid id);
        Model? GetModelById(Guid id);
        Department? GetDepartmentById(Guid id);
        bool IsVinAvailable(string vin);
        bool IsRegistrationAvailable(string registration);
        Vehicle? GetVehicleById(Guid id);
        List<Vehicle> GetFilteredVehicles(GetFilteredVehiclesDtoRequest dto);
        bool UpdateVehicle(Guid id, Vehicle dto);
        bool CreateVehicle(Vehicle dto);
        Guid GetPriceId(double pricePerDay);
        bool DeleteVehicle(Guid id);
        Department? GetVehicleDepartment(Guid vehicleId);
    }
}