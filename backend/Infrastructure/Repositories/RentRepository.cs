using Application.Constants;
using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Repositories;

public class RentRepository : IRentRepository
{
    private readonly DataContext _context;

    public RentRepository(DataContext context)
    {
        _context = context;
    }

    public bool CreateRent(Rent rent)
    {
        _context.Rents.Add(rent);
        var isCreated = _context.SaveChanges() > 0;
        return isCreated;
    }

    public List<Rent> GetDepartmentArchivedRents(Guid departmentId)
    {
        var rents = _context.Rents
            .Include(x => x.RentStatus)
            .Where(x => x.RentStatus.Name.Equals(RentStatuses.Archived))
            .ToList();

        return rents;
    }

    public Department? GetDepartmentById(Guid id)
    {
        var department = _context.Departments
            .Include(x => x.Employees)
            .FirstOrDefault(x => x.Id.Equals(id));

        return department;
    }

    public List<Rent> GetDepartmentRents(Guid departmentId)
    {
        var rents = _context.Vehicles
            .Include(x => x.Rentings)
            .ThenInclude(x => x.RentStatus)
            .Where(x => x.DepartmentId.Equals(departmentId))
            .SelectMany(x => x.Rentings)
            .Where(x => x.RentStatus.Name.Equals(RentStatuses.Active) 
                || x.RentStatus.Name.Equals(RentStatuses.Reserved))
            .ToList();

        return rents;
    }

    public Rent? GetRentById(Guid id)
    {
        var rent = _context.Rents
            .Include(x => x.Client)
            .ThenInclude(x => x.UserData)
            .Include(x => x.Receiver)
            .ThenInclude(x => x.UserData)
            .Include(x => x.Renter)
            .ThenInclude(x => x.UserData)
            .Include(x => x.Vehicle)
            .ThenInclude(x => x.Model)
            .ThenInclude(x => x.Brand)
            .FirstOrDefault(x => x.Id.Equals(id));

        return rent;
    }

    public Guid? GetRentDepartmentId(Guid rentId)
    {
        var departmentId = _context.Rents
            .Include(x => x.Vehicle)
            .Where(x => x.Id.Equals(rentId))
            .Select(x => x.Vehicle.DepartmentId)
            .FirstOrDefault();

        return departmentId;
    }

    public Guid? GetRentStatusIdByName(string statusName)
    {
        var rentStatusId = _context.RentStatuses
            .Where(x => x.Name.Equals(statusName))
            .Select(x => x.Id)
            .FirstOrDefault();

        return rentStatusId;
    }

    public List<Rent> GetUserArchivedRents(Guid userId)
    {
        var rents = _context.Users
            .Where(x => x.Id.Equals(userId))
            .Include(x => x.OwnRentings)
            .ThenInclude(x => x.RentStatus)
            .SelectMany(x => x.OwnRentings)
            .Where(x => x.RentStatus.Name.Equals(RentStatuses.Archived))
            .ToList();

        return rents;
    }

    public List<Rent> GetUserRents(Guid userId)
    {
        var rents = _context.Users
            .Where(x => x.Id.Equals(userId))
            .Include(x => x.OwnRentings)
            .ThenInclude(x => x.RentStatus)
            .SelectMany(x => x.OwnRentings)
            .Where(x => x.RentStatus.Name.Equals(RentStatuses.Active) 
                || x.RentStatus.Name.Equals(RentStatuses.Reserved))
            .ToList();

        return rents;
    }

    public double? GetVehiclePrice(Guid vehicleId)
    {
        var price = _context.Vehicles
            .Where(x => x.Id.Equals(vehicleId))
            .Include(x => x.Price)
            .Select(x => x.Price.PricePerDay)
            .FirstOrDefault();

        return price;
    }

    public bool UpdateRent(Rent rent)
    {
        var rentToUpdate = _context.Rents.Find(rent.Id);

        if (rentToUpdate is null)
            return false;

        _context.Rents
            .Update(rentToUpdate)
            .CurrentValues.SetValues(rent);

        var isUpdated = _context.SaveChanges() > 0;

        return isUpdated;
    }
}
