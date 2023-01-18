namespace Application.Dtos.Rent;

public class GetRentByIdDtoResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = default!;
    public string Client { get; set; } = default!;
    public Guid ClientId { get; set; } = default!;
    public string Renter { get; set; } = default!;
    public Guid RenterId { get; set; } = default!;
    public string Receiver { get; set; } = default!;
    public Guid ReceiverId { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public VehicleForGetRentByIdDtoResponse Vehicle { get; set; } = default!;
}

public class VehicleForGetRentByIdDtoResponse
{
    public Guid Id { get; set; }
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public int YearOfProduction { get; set; }
}