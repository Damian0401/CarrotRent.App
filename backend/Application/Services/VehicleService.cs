using Application.Dtos.Vehicle;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;

namespace Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper, IUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public GetFilteredVehiclesDtoResponse GetFilteredVehicles(GetFilteredVehiclesDtoRequest dto)
    {
        var vehicles = _vehicleRepository
            .GetFilteredVehicles(dto);

        var mappedVehicles = _mapper.Map<List<VehicleForGetFilteredVehiclesDtoResponse>>(vehicles);
        var response = new GetFilteredVehiclesDtoResponse
        {
            Vehicles = mappedVehicles
        };

        return response;
    }

    public GetVehicleByIdDtoResponse? GetVehicleById(Guid id)
    {
        var vehicle = _vehicleRepository.GetVehicleById(id);

        if (vehicle is null)
            return null;

        var response = _mapper.Map<GetVehicleByIdDtoResponse>(vehicle);

        return response;
    }

    public bool UpdateVehicle(Guid id, UpdateVehicleDtoRequest dto)
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();

        if (user is null)
            return false;

        var department = _vehicleRepository
            .GetVehicleDepartment(id);

        if (department is null)
            return false;

        if (!user.Id.Equals(department.ManagerId) && 
            !department.Employees.Any(x => x.Id.Equals(user.Id)))
            return false;

        var mappedVehicle = _mapper.Map<Vehicle>(dto);

        var isUpdated = _vehicleRepository.UpdateVehicle(id, mappedVehicle);

        return isUpdated;
    }

    public bool DeleteVehicle(Guid id)
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();

        if (user is null)
            return false;

        var department = _vehicleRepository
            .GetVehicleDepartment(id);

        if (department is null)
            return false;

        if (!department.ManagerId.Equals(user.Id))
            return false;

        var isDeleted = _vehicleRepository
            .DeleteVehicle(id);

        return isDeleted;
    }

    public bool CreateVehicle(CreateVehicleDtoRequest dto)
    {
        bool isVehicleValid = ValidateVehicle(dto);

        if (!isVehicleValid)
            return false;

        var vehicleToCreate = _mapper.Map<Vehicle>(dto);
        vehicleToCreate.PriceId = _vehicleRepository.GetPriceId(dto.Price);

        bool isCreated = _vehicleRepository.CreateVehicle(vehicleToCreate);

        return isCreated;
    }

    public GetVehicleFilterDataDtoResponse GetVehicleFilterData()
    {
        var fuels = _vehicleRepository.GetAllFuels();
        var brands = _vehicleRepository.GetAllBrands();
        var departments = _vehicleRepository.GetAllDepartments();

        var mappedFuels = _mapper.Map<List<FuelForGetVehicleFilterDataDtoResponse>>(fuels);
        var mappedBrands = _mapper.Map<List<BrandForGetVehicleFilterDataDtoResponse>>(brands);
        var mappedDepartments = _mapper.Map<List<DepartmentForGetVehicleFilterDataDtoResponse>>(departments);
        
        var response = new GetVehicleFilterDataDtoResponse
        {
            Fuels = mappedFuels,
            Brands = mappedBrands,
            Departments = mappedDepartments,
        };

        return response;
    }

    private bool ValidateVehicle(CreateVehicleDtoRequest dto)
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();

        if (user is null)
            return false;

        var department = _vehicleRepository.GetDepartmentById(dto.DepartmentId);
        if (department is null || !department.ManagerId.Equals(user.Id))
            return false;

        if (!_vehicleRepository.IsVinAvailable(dto.Vin))
            return false;

        if (!_vehicleRepository.IsRegistrationAvailable(dto.Registration))
            return false;

        var fuel = _vehicleRepository.GetFuelById(dto.FuelId);
        if (fuel is null)
            return false;

        var model = _vehicleRepository.GetModelById(dto.ModelId);
        if (model is null)
            return false;

        return true;
    }
}