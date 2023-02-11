namespace Application.Dtos.Rent;

public class GetDepartmentArchivedRentsDtoResponse
{
    public List<RentForGetDepartmentArchivedRentsDtoResponse> Rents { get; set; } = default!;
}

public class RentForGetDepartmentArchivedRentsDtoResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Vehicle { get; set; } = default!;
}