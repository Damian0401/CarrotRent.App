namespace Application.Dtos.Account
{
    public class LoginDtoRequest
    {
        public string Login { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}