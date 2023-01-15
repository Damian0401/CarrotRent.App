using System;
using Application.Core;
using Application.Dtos.Account;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace UnitTests.AccountServiceTests;

public class Login
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly Mock<IJwtGenerator> _JwtGeneratorMock;
    private readonly AccountService _accountService;

    public Login()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _accountRepositoryMock = new();
        _userAccessorMock = new();
        _JwtGeneratorMock = new();

        _accountService = new AccountService(_accountRepositoryMock.Object, mapper,
            _JwtGeneratorMock.Object, _userAccessorMock.Object);
    }

    [Theory]
    [InlineData("user password", "request password")]
    [InlineData("1234567890", "0987654321")]
    [InlineData("c9fb75fb-6d57-4343-bee8-0506d87d7716", "05d3c707-af4e-42e7-be43-4faa1453c076")]
    public void Login_DiffrentPassword_ReturnsNull(string userPassword, string requestPassword)
    {
        // Arrange
        var user = new User();
        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, userPassword);

        _accountRepositoryMock.Setup(x => x.GetUserByLogin(It.IsAny<string>())).Returns(user);

        var request = new LoginDtoRequest
        {
            Login = "RequestLogin",
            Password = requestPassword
        };

        // Act
        var result = _accountService.Login(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Login_UserNotFound_ReturnsNull()
    {
        // Arrange
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.GetUserByLogin(It.IsAny<string>())).Returns<LoginDtoRequest?>(null);

        var jwtGeneratorMock = new Mock<IJwtGenerator>();
        var userAccessorMock = new Mock<IUserAccessor>();

        var request = new LoginDtoRequest { Login = "UserLogin" };

        // Act
        var result = _accountService.Login(request);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData("user password")]
    [InlineData("1234567890")]
    [InlineData("c9fb75fb-6d57-4343-bee8-0506d87d7716")]
    public void Login_CorrectRequest_ReturnsResponse(string userPassword)
    {
        // Arrange
        var user = new User();
        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, userPassword);

        var jwtGeneratorMock = new Mock<IJwtGenerator>();
        var userAccessorMock = new Mock<IUserAccessor>();

        _accountRepositoryMock.Setup(x => x.GetUserByLogin(It.IsAny<string>())).Returns(user);

        var request = new LoginDtoRequest { Password = userPassword };

        // Act
        var result = _accountService.Login(request);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Login_CorrectRequest_ReturnsCorrectLogin()
    {
        // Arrange
        var userLogin = "UserLogin";
        var password = "UserPassword";
        var user = new User()
        {
            Login = userLogin,
        };
        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, password);

        _accountRepositoryMock.Setup(x => x.GetUserByLogin(userLogin)).Returns(user);

        var request = new LoginDtoRequest
        {
            Login = userLogin,
            Password = password
        };

        // Act
        var result = _accountService.Login(request);

        // Assert
        Assert.Equal(userLogin, result!.Login);
    }
}