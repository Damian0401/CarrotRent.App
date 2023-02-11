using Domain.Models;

namespace Application.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(User user, DateTime expireDate);
    }
}