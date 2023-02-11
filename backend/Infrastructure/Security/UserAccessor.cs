using System.Security.Claims;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserAccessor(DataContext context, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _context = context;
        }

        public User? GetCurrentlyLoggedUser()
        {
            var userId = _contextAccessor
                .HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return null;

            var userIdAsGuid = Guid.Parse(userId);

            var user = _context.Users
                .Include(x => x.Role)
                .Include(x => x.OwnedDepartments)
                .FirstOrDefault(x => x.Id.Equals(userIdAsGuid));

            return user;
        }
    }
}