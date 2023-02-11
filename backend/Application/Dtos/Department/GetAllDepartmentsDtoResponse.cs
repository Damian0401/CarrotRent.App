namespace Application.Dtos.Department;

public class GetAllDepartmentsDtoResponse
{
    public List<DepartmentForGetAllDepartmentsDtoResponse> Departments { get; set; } = default!;
}

public class DepartmentForGetAllDepartmentsDtoResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public double XPosition { get; set; } = default!;
    public double YPosition { get; set; } = default!;
}