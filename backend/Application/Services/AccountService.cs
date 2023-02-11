using System.Linq;
using Application.Constants;
using Application.Dtos.Account;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IUserAccessor _userAccessor;

    public AccountService(IAccountRepository accountRepository, IMapper mapper,
        IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
        _jwtGenerator = jwtGenerator;
        _mapper = mapper;
        _accountRepository = accountRepository;
    }

    public GetUnverifiedUsersDtoResponse GetUnverifiedUsers()
    {
        var users = _accountRepository.GetUnverifiedUsers();

        var mappedUsers = _mapper.Map<List<UserForGetUnverifiedUsersDtoResponse>>(users);

        var response = new GetUnverifiedUsersDtoResponse { Users = mappedUsers };

        return response;
    }

    public LoginDtoResponse? Login(LoginDtoRequest dto)
    {
        var user = _accountRepository.GetUserByLogin(dto.Login);

        if (user is null)
            return null;

        bool isPasswordCorrect = IsPasswordCorrect(user, dto.Password);

        if (!isPasswordCorrect)
            return null;

        var response = _mapper.Map<LoginDtoResponse>(user);
        response.Token = _jwtGenerator.CreateToken(user, DateTime.Now.AddDays(3));

        return response;
    }

    public RegisterDtoResponse? Register(RegisterDtoRequest dto)
    {
        bool isUserValid = ValidateUser(dto.Email, dto.Login, dto.Pesel, dto.PhoneNumber);

        if (!isUserValid)
            return null;

        var address = _mapper.Map<Address>(dto);

        var userData = _mapper.Map<UserData>(dto);
        userData.Address = address;

        var user = _mapper.Map<User>(dto);
        user.UserData = userData;
        user.PasswordHash = GeneratePasswordHash(user, dto.Password);
        user.Role = _accountRepository.GetRoleByName(Roles.Unverified);

        bool isCreated = _accountRepository.CreateUser(user);

        if (!isCreated)
            return null;

        var response = _mapper.Map<RegisterDtoResponse>(user);
        response.Token = _jwtGenerator.CreateToken(user, DateTime.Now.AddDays(3));

        return response;
    }

    public bool CreateEmployee(CreateEmployeeDtoRequest dto, Guid departmentId)
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();

        if (user is null || !user.Role.Name.Equals(Roles.Manager))
            return false;

        if (!user.OwnedDepartments.Any(x => x.Id.Equals(departmentId)))
            return false;

        bool isEmployeeValid = ValidateUser(dto.Email, dto.Login, dto.Pesel, dto.PhoneNumber);

        if (!isEmployeeValid)
            return false;

        var address = _mapper.Map<Address>(dto);

        var employeeData = _mapper.Map<UserData>(dto);
        employeeData.Address = address;

        var employee = _mapper.Map<User>(dto);
        employee.UserData = employeeData;
        employee.PasswordHash = GeneratePasswordHash(employee, dto.Password);
        employee.Role = _accountRepository.GetRoleByName(Roles.Employee);
        employee.DepartmentId = departmentId;

        bool isCreated = _accountRepository.CreateUser(employee);

        if (!isCreated)
            return false;

        return true;
    }

    public bool VerifyUser(Guid userId)
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();

        if (user is null)
            return false;

        if (!user.Role.Name.Equals(Roles.Employee) && !user.Role.Name.Equals(Roles.Manager))
            return false;

        var userToVerifyRole = _accountRepository.GetUserRoleName(userId);

        if (userToVerifyRole is null || !userToVerifyRole.Equals(Roles.Unverified))
            return false;

        var clientRole = _accountRepository.GetRoleByName(Roles.Client);

        if (clientRole is null)
            return false;

        var isUpdated = _accountRepository.UpdateUserRoleId(userId, clientRole.Id);

        return isUpdated;
    }

    private string GeneratePasswordHash(User user, string password)
    {
        var hasher = new PasswordHasher<User>();

        var passwordHash = hasher.HashPassword(user, password);

        return passwordHash;
    }

    private bool IsPasswordCorrect(User user, string password)
    {
        var hasher = new PasswordHasher<User>();

        var isPasswordValid = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

        return isPasswordValid.Equals(PasswordVerificationResult.Success);
    }

    private bool ValidateUser(string email, string login, string pesel, string phoneNumber)
    {
        if (!_accountRepository.IsEmailAvailable(email))
            return false;

        if (!_accountRepository.IsLoginAvailable(login))
            return false;

        if (!_accountRepository.IsPeselAvailable(pesel))
            return false;

        if (!_accountRepository.IsPhoneNumberAvailable(phoneNumber))
            return false;

        return true;
    }
}