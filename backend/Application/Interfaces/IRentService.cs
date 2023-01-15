using Application.Dtos.Rent;

namespace Application.Interfaces
{
    public interface IRentService
    {
        bool CreateRent(CreateRentDtoRequest dto);
        bool IssueRent(Guid rentId);
        ReceiveRentDtoResponse? ReceiveRent(Guid rentId);
        GetRentByIdDtoResponse? GetRentById(Guid rentId);
        GetMyRentsDtoResponse? GetMyRents();
        GetMyArchivedRentsDtoResponse? GetMyArchivedRents();
        GetDepartmentRentsDtoResponse? GetDepartmentRents(Guid departmentId);
        GetDepartmentArchivedRentsDtoResponse? GetDepartmentArchivedRents(Guid departmentId);
        bool CancelRent(Guid rentId);
    }
}