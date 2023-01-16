using Application.Core;
using Application.Dtos.Account;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.AccountServiceTests;

public class Register
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly Mock<IJwtGenerator> _JwtGeneratorMock;
    private readonly AccountService _accountService;

    public Register()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _accountRepositoryMock = new();
        _userAccessorMock = new();
        _JwtGeneratorMock = new();

        _accountService = new(_accountRepositoryMock.Object, mapper, 
            _JwtGeneratorMock.Object, _userAccessorMock.Object);
    }

    [Fact]
    public void Register_EmailNotAvailable_ReturnsNull()
    {
        // Arrange
        var email = "RequestEmail";
        var request = new RegisterDtoRequest { Email = email };

        _accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(false);

        // Act
        var result = _accountService.Register(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Register_LoginNotAvailable_ReturnsNull()
    {
        // Arrange
        var email = "RequestEmail";
        var login = "RequestLogin";
        var request = new RegisterDtoRequest
        {
            Email = email,
            Login = login
        };

        _accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsLoginAvailable(login)).Returns(false);

        // Act
        var result = _accountService.Register(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Register_PeselNotAvailable_ReturnsNull()
    {
        // Arrange
        var email = "RequestEmail";
        var login = "RequestLogin";
        var pesel = 1234567890;
        var request = new RegisterDtoRequest
        {
            Email = email,
            Login = login,
            Pesel = pesel
        };

        _accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsLoginAvailable(login)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsPeselAvailable(pesel)).Returns(false);

        // Act
        var result = _accountService.Register(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Register_PhoneNumberNotAvailable_ReturnsNull()
    {
        // Arrange
        var email = "RequestEmail";
        var login = "RequestLogin";
        var pesel = 1234567890;
        var phoneNumber = "1234567890";
        var request = new RegisterDtoRequest
        {
            Email = email,
            Login = login,
            Pesel = pesel,
            PhoneNumber = phoneNumber
        };

        _accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsLoginAvailable(login)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsPeselAvailable(pesel)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsPhoneNumberAvailable(phoneNumber)).Returns(false);

        // Act
        var result = _accountService.Register(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Register_UserNotCreated_ReturnsNull()
    {
        // Arrange
        var email = "RequestEmail";
        var login = "RequestLogin";
        var pesel = 1234567890;
        var phoneNumber = "1234567890";
        var request = new RegisterDtoRequest
        {
            Email = email,
            Login = login,
            Pesel = pesel,
            PhoneNumber = phoneNumber,
            Password = "UserPassword"
        };

        _accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsLoginAvailable(login)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsPeselAvailable(pesel)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsPhoneNumberAvailable(phoneNumber)).Returns(true);
        _accountRepositoryMock.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(false);

        // Act
        var result = _accountService.Register(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Register_CorrectRequest_ReturnsResponse()
    {
        // Arrange
        var email = "RequestEmail";
        var login = "RequestLogin";
        var pesel = 1234567890;
        var phoneNumber = "1234567890";
        var request = new RegisterDtoRequest
        {
            Email = email,
            Login = login,
            Pesel = pesel,
            PhoneNumber = phoneNumber,
            Password = "UserPassword"
        };

        _accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsLoginAvailable(login)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsPeselAvailable(pesel)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsPhoneNumberAvailable(phoneNumber)).Returns(true);
        _accountRepositoryMock.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(true);

        // Act
        var result = _accountService.Register(request);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Register_CorrectRequest_ReturnsCorrectLogin()
    {
        // Arrange
        var email = "RequestEmail";
        var login = "RequestLogin";
        var pesel = 1234567890;
        var phoneNumber = "1234567890";
        var request = new RegisterDtoRequest
        {
            Email = email,
            Login = login,
            Pesel = pesel,
            PhoneNumber = phoneNumber,
            Password = "UserPassword"
        };

        _accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsLoginAvailable(login)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsPeselAvailable(pesel)).Returns(true);
        _accountRepositoryMock.Setup(x => x.IsPhoneNumberAvailable(phoneNumber)).Returns(true);
        _accountRepositoryMock.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(true);

        // Act
        var result = _accountService.Register(request);

        // Assert
        Assert.Equal(login, result!.Login);
    }
}