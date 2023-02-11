namespace Application.Dtos.Account
{
    public class RegisterDtoResponse
    {
        public string Login { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string Role { get; set; } = default!;
        public List<Guid> DepartmentIds { get; set; } = new();
    }
}