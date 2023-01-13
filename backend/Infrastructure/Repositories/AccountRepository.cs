using Application.Constants;
using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly DataContext _context;
    public AccountRepository(DataContext context)
    {
        _context = context;
    }

    public bool CreateUser(User user)
    {
        _context.Users.Add(user);

        var result = _context.SaveChanges();

        return result > 0;
    }

    public Role GetRoleByName(string roleName)
    {
        var role = _context
            .Roles
            .FirstOrDefault(r => r.Name.Equals(roleName));

        if (role is not null)
        {
            return role;
        }

        role = new Role
        {
            Name = roleName,
        };
        _context.Roles.Add(role);
        _context.SaveChanges();

        return role;
    }

    public User? GetUserByLogin(string login)
    {
        return _context
            .Users
            .Include(x => x.Role)
            .Include(x => x.OwnedDepartments)
            .FirstOrDefault(x => x.Login.Equals(login));
    }

    public bool IsEmailAvailable(string email)
    {
        return !_context
            .UserData
            .Any(x => x.Email.Equals(email));
    }

    public bool IsLoginAvailable(string login)
    {
        return !_context
            .Users
            .Any(x => x.Login.Equals(login));
    }

    public bool IsPeselAvailable(int pesel)
    {
        return !_context
            .UserData
            .Any(x => x.Pesel.Equals(pesel));
    }

    public bool IsPhoneNumberAvailable(string phoneNumber)
    {
        return !_context
            .UserData
            .Any(x => x.PhoneNumber.Equals(phoneNumber));
    }
}
