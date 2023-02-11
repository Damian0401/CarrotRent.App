namespace Domain.Models;

public class Localization
{
    public Guid Id { get; set; }
    public double XPosition { get; set; }
    public double YPosition { get; set; }

    public Department Department { get; set; } = default!;
}