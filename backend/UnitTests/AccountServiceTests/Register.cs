using System;
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
    private readonly IMapper _mapper;
    private readonly IJwtGenerator _jwtGenerator;

    public Register()
    {
        var jwtGeneratorMock = new Mock<IJwtGenerator>();
        jwtGeneratorMock.Setup(x => x.CreateToken(It.IsAny<User>(), It.IsAny<DateTime>()))
            .Returns("token");
        _jwtGenerator = jwtGeneratorMock.Object;

        _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();
    }

    [Fact]
    public void Register_EmailNotAvailable_ReturnsNull()
    {
        // Arrange
        var email = "RequestEmail";
        var request = new RegisterDtoRequest { Email = email };

        var userAccessorMock = new Mock<IUserAccessor>();
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(false);

        var accountService = new AccountService(accountRepositoryMock.Object, 
            _mapper, _jwtGenerator, userAccessorMock.Object);

        // Act
        var result = accountService.Register(request);

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

        var userAccessorMock = new Mock<IUserAccessor>();
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(true);
        accountRepositoryMock.Setup(x => x.IsLoginAvailable(login)).Returns(false);

        var accountService = new AccountService(accountRepositoryMock.Object, 
            _mapper, _jwtGenerator, userAccessorMock.Object);


        // Act
        var result = accountService.Register(request);

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

        var userAccessorMock = new Mock<IUserAccessor>();
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(true);
        accountRepositoryMock.Setup(x => x.IsLoginAvailable(login)).Returns(true);
        accountRepositoryMock.Setup(x => x.IsPeselAvailable(pesel)).Returns(false);

        var accountService = new AccountService(accountRepositoryMock.Object, 
            _mapper, _jwtGenerator, userAccessorMock.Object);

        // Act
        var result = accountService.Register(request);

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

        var userAccessorMock = new Mock<IUserAccessor>();
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(true);
        accountRepositoryMock.Setup(x => x.IsLoginAvailable(login)).Returns(true);
        accountRepositoryMock.Setup(x => x.IsPeselAvailable(pesel)).Returns(true);
        accountRepositoryMock.Setup(x => x.IsPhoneNumberAvailable(phoneNumber)).Returns(false);

        var accountService = new AccountService(accountRepositoryMock.Object, 
            _mapper, _jwtGenerator, userAccessorMock.Object);

        // Act
        var result = accountService.Register(request);

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

        var userAccessorMock = new Mock<IUserAccessor>();
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.IsEmailAvailable(email)).Returns(true);
        accountRepositoryMock.Setup(x => x.IsLoginAvailable(login)).Returns(true);
        accountRepositoryMock.Setup(x => x.IsPeselAvailable(pesel)).Returns(true);
        accountRepositoryMock.Setup(x => x.IsPhoneNumberAvailable(phoneNumber)).Returns(false);

        var accountService = new AccountService(accountRepositoryMock.Object, 
            _mapper, _jwtGenerator, userAccessorMock.Object);

        // Act
        var result = accountService.Register(request);

        // Assert
        Assert.Null(result);
    }
}