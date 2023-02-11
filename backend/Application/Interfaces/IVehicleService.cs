using Application.Dtos.Vehicle;

namespace Application.Interfaces
{
    public interface IVehicleService
    {
        GetFilteredVehiclesDtoResponse GetFilteredVehicles(GetFilteredVehiclesDtoRequest dto);
        GetVehicleByIdDtoResponse? GetVehicleById(Guid id);
        GetVehicleFilterDataDtoResponse GetVehicleFilterData();
        bool UpdateVehicle(Guid id, UpdateVehicleDtoRequest dto);
        bool DeleteVehicle(Guid id);
        bool CreateVehicle(CreateVehicleDtoRequest dto);
    }
}