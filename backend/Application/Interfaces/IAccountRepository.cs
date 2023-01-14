using Domain.Models;

namespace Application.Interfaces
{
    public interface IAccountRepository
    {
        bool IsEmailAvailable(string email);
        bool IsPeselAvailable(int pesel);
        bool IsPhoneNumberAvailable(string phoneNumber);
        bool IsLoginAvailable(string login);
        bool CreateUser(User user);
        bool UpdateUserRoleId(Guid userId, Guid roleId);
        string? GetUserRoleName(Guid userId);
        User? GetUserByLogin(string login);
        Role GetRoleByName(string roleName);
    }
}