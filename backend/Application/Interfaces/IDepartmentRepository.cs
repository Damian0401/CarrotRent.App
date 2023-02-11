using Domain.Models;

namespace Application.Interfaces
{
    public interface IDepartmentRepository
    {
        List<Department> GetAllDepartments();
        Department? GetDepartmentById(Guid id);
    }
}