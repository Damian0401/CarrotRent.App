using Application.Constants;
using Application.Dtos.Vehicle;
using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly DataContext _context;

    public VehicleRepository(DataContext context)
    {
        _context = context;
    }

    public bool CreateVehicle(Vehicle dto)
    {
        var status = _context.VehicleStatuses
            .FirstOrDefault(x => x.Name.Equals(VehicleStatuses.Available));
        
        if (status is null)
        {
            status = new VehicleStatus
            {
                Name = VehicleStatuses.Available
            };
        }

        dto.VehicleStatus = status;
        _context.Vehicles.Add(dto);
        var isCreated = _context.SaveChanges() > 0;
        return isCreated;
    }

    public bool DeleteVehicle(Guid id)
    {
        var vehicleToDelete = _context
            .Vehicles
            .Find(id);

        if (vehicleToDelete is null)
            return false;

        _context.Vehicles.Remove(vehicleToDelete);
        var isDeleted = _context.SaveChanges() > 0;
        return isDeleted;
    }

    public List<Brand> GetAllBrands()
    {
        var brands = _context.Brands
            .AsQueryable();

        brands = brands.Include(x => x.Models);

        return brands.ToList();
    }

    public List<Department> GetAllDepartments()
    {
        var departments = _context.Departments
            .ToList();

        return departments;
    }

    public List<Fuel> GetAllFuels()
    {
        var fuels = _context.Fuels
            .ToList();

        return fuels;
    }

    public Department? GetDepartmentById(Guid id)
    {
        return _context
            .Departments
            .Find(id);
    }

    public List<Vehicle> GetFilteredVehicles(GetFilteredVehiclesDtoRequest dto)
    {
        var vehicles = _context.Vehicles
            .Include(x => x.Model)
            .ThenInclude(x => x.Brand)
            .Include(x => x.Fuel)
            .Include(x => x.Price)
            .Include(x => x.Department)
            .AsQueryable();

        if (dto.BrandId is not null)
            vehicles = vehicles.Where(x =>
                x.Model.Brand.Id.Equals(dto.BrandId));

        if (dto.ModelId is not null)
            vehicles = vehicles.Where(x => x.ModelId.Equals(dto.ModelId));

        if (dto.FuelId is not null)
            vehicles = vehicles.Where(x =>
                x.Fuel.Id.Equals(dto.FuelId));

        if (dto.DepartmentId is not null)
            vehicles = vehicles.Where(x => x.Department.Id.Equals(dto.DepartmentId));

        if (dto.Seats is not null)
            vehicles = vehicles.Where(x =>
                x.Seats.Equals(dto.Seats));

        if (dto.MinPrice is not null)
            vehicles = vehicles.Where(x =>
                x.Price.PricePerDay >= dto.MinPrice);

        if (dto.MaxPrice is not null)
            vehicles = vehicles.Where(x =>
                x.Price.PricePerDay <= dto.MaxPrice);

        return vehicles.ToList();
    }

    public Fuel? GetFuelById(Guid id)
    {
        return _context
            .Fuels
            .Find(id);
    }

    public Model? GetModelById(Guid id)
    {
        return _context
            .Models
            .Find(id);
    }

    public Guid GetPriceId(double pricePerDay)
    {
        var price = _context.Prices
            .FirstOrDefault(x => x.PricePerDay.Equals(pricePerDay));

        if (price is null)
        {
            price = new Price
            {
                PricePerDay = pricePerDay,
            };
            _context.Prices.Add(price);
            _context.SaveChanges();
        }

        return price.Id;
    }

    public Vehicle? GetVehicleById(Guid id)
    {
        var vehicle = _context.Vehicles
            .Include(x => x.Model)
            .ThenInclude(x => x.Brand)
            .Include(x => x.Price)
            .Include(x => x.VehicleStatus)
            .Include(x => x.Department)
            .Include(x => x.Fuel)
            .FirstOrDefault(x => x.Id.Equals(id));

        return vehicle;
    }

    public Department? GetVehicleDepartment(Guid vehicleId)
    {
        var department = _context
            .Vehicles
            .Include(x => x.Department)
            .ThenInclude(x => x.Employees)
            .Where(x => x.Id.Equals(vehicleId))
            .Select(x => x.Department)
            .FirstOrDefault();

        return department;
    }

    public bool IsRegistrationAvailable(string registration)
    {
        return !_context
            .Vehicles
            .Any(x => x.Registration.Equals(registration));
    }

    public bool IsVinAvailable(string vin)
    {
        return !_context
            .Vehicles
            .Any(x => x.Vin.Equals(vin));
    }

    public bool UpdateVehicle(Guid id, Vehicle dto)
    {
        var vehicle = _context.Vehicles.Find(id);

        if (vehicle is null)
            return false;

        vehicle.PriceId = GetPriceId(dto.Price.PricePerDay);
        vehicle.Description = dto.Description;
        vehicle.Seats = dto.Seats;
        vehicle.YearOfProduction = dto.YearOfProduction;
        vehicle.ImageUrl = dto.ImageUrl;

        var isUpdated = _context.SaveChanges() > 0;
        return isUpdated;
    }
}
