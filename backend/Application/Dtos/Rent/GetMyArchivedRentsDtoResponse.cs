namespace Application.Dtos.Rent;

public class GetMyArchivedRentsDtoResponse
{
    public List<RentForGetMyArchivedRentsDtoResponse> Rents { get; set; } = default!;
}

public class RentForGetMyArchivedRentsDtoResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Vehicle { get; set; } = default!;
}