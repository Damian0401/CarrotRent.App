using Application.Dtos.Rent;

namespace Application.Interfaces
{
    public interface IRentService
    {
        bool CreateRent(CreateRentDtoRequest dto);
        bool IssueRent(Guid id);
        ReceiveRentDtoResponse? ReceiveRent(Guid id);
        GetRentByIdDtoResponse? GetRentById(Guid id);
        GetMyRentsDtoResponse? GetMyRents();
        GetMyArchivedRentsDtoResponse? GetMyArchivedRents();
        GetDepartmentRentsDtoResponse? GetDepartmentRents(Guid departmentId);
        GetDepartmentArchivedRentsDtoResponse? GetDepartmentArchivedRents(Guid departmentId);
        bool CancelRent(Guid id);
    }
}