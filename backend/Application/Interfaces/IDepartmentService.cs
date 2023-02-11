using Application.Dtos.Department;

namespace Application.Interfaces
{
    public interface IDepartmentService
    {
        GetAllDepartmentsDtoResponse GetAllDepartments();
        GetDepartmentByIdDtoResponse? GetDepartmentById(Guid id);
    }
}