namespace Application.Dtos.Rent;

public class CreateRentDtoRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid VehicleId { get; set; }
}