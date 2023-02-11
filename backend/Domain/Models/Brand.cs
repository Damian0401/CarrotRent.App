namespace Domain.Models;

public class Brand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<Model> Models { get; set; } = default!;
}