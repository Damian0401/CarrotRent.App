
namespace Domain.Models;

public class Address
{
    public Guid Id { get; set; }
    public string PostCode { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string HouseNumber { get; set; } = default!;
    public string ApartmentNumber { get; set; } = default!;

    public UserData UserData { get; set; } = default!;
    public Department Department { get; set; } = default!;
}