namespace Application.Dtos.Account;

public class GetUnverifiedUsersDtoResponse
{
    public List<UserForGetUnverifiedUsersDtoResponse> Users { get; set; } = default!;
}

public class UserForGetUnverifiedUsersDtoResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Pesel { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
}