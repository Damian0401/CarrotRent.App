namespace Domain.Models;

public class RentStatus
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<Rent> Rentings { get; set; } = default!;
}