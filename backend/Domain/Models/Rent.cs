namespace Domain.Models;

public class Rent
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid ClientId { get; set; }
    public Guid? RenterId { get; set; }
    public Guid? ReceiverId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid RentStatusId { get; set; }

    public User Client { get; set; } = default!;
    public User Renter { get; set; } = default!;
    public User Receiver { get; set; } = default!;
    public Vehicle Vehicle { get; set; } = default!;
    public RentStatus RentStatus { get; set; } = default!;
}