namespace Domain.Models;

public class Department
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid? ManagerId { get; set; }
    public Guid AddressId { get; set; } = default!;
    public Guid LocalizationId { get; set; }

    public Localization Localization { get; set; } = default!;
    public Address Address { get; set; } = default!;
    public User Manager { get; set; } = default!;
    public ICollection<Vehicle> Vehicles { get; set; } = default!;
    public ICollection<User> Employees { get; set; } = default!;
}