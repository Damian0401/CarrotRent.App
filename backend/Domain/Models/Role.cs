namespace Domain.Models;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<User> Users { get; set; } = default!;
}