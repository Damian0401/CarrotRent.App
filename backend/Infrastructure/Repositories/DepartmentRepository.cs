using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly DataContext _context;

    public DepartmentRepository(DataContext context)
    {
        _context = context;
    }

    public List<Department> GetAllDepartments()
    {
        var departments = _context.Departments
            .Include(d => d.Address)
            .Include(d => d.Localization)
            .ToList();

        return departments;
    }

    public Department? GetDepartmentById(Guid id)
    {
        var department = _context.Departments
            .Include(d => d.Address)
            .Include(d => d.Manager)
                .ThenInclude(x => x.UserData)
            .Include(d => d.Vehicles)
                .ThenInclude(x => x.Model)
                    .ThenInclude(x => x.Brand)
            .FirstOrDefault(x => x.Id.Equals(id));

        return department;
    }
}
