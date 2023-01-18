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
        bool isUserValid = ValidateUser(dto);

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

    private bool ValidateUser(RegisterDtoRequest dto)
    {
        if (!_accountRepository.IsEmailAvailable(dto.Email))
            return false;

        if (!_accountRepository.IsLoginAvailable(dto.Login))
            return false;

        if (!_accountRepository.IsPeselAvailable(dto.Pesel))
            return false;

        if (!_accountRepository.IsPhoneNumberAvailable(dto.PhoneNumber))
            return false;

        return true;
    }
}