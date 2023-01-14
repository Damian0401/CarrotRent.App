using Application.Dtos.Account;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        LoginDtoResponse? Login(LoginDtoRequest dto);
        RegisterDtoResponse? Register(RegisterDtoRequest dto);
        bool Verify(Guid userId);
    }
}