using Domain.Models;

namespace Application.Interfaces;

public interface IRentRepository
{
    bool CreateRent(Rent rent);
    Rent? GetRentById(Guid id);
    Guid? GetRentStatusIdByName(string statusName);
    Department? GetDepartmentById(Guid id);
    List<Rent> GetDepartmentArchivedRents(Guid departmentId);
    List<Rent> GetDepartmentRents(Guid departmentId);
    List<Rent> GetUserArchivedRents(Guid userId);
    List<Rent> GetUserRents(Guid userId);
    Guid? GetRentDepartmentId(Guid rentId);
    bool UpdateRent(Rent rent);
    double? GetVehiclePrice(Guid vehicleId);
    List<Rent> GetRentBetweenDates(Guid vehicleId, DateTime startDate, DateTime endDate);
}