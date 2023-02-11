using Domain.Models;

namespace Application.Interfaces
{
    public interface IUserAccessor
    {
        User? GetCurrentlyLoggedUser();
    }
}