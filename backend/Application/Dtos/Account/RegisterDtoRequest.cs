namespace Application.Dtos.Account
{
    public class RegisterDtoRequest
    {
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Pesel { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PostCode { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string HouseNumber { get; set; } = default!;
        public string ApartmentNumber { get; set; } = default!;
    }
}