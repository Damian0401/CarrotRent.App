using System;
using Application.Constants;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.AccountServiceTests;

public class Verify
{
    private readonly IMapper _mapper;

    public Verify()
    {
        _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();
    }

    [Fact]
    public void Verify_UserNotLogged_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var accountRepositoryMock = new Mock<IAccountRepository>();
        var jwtGeneratorMock = new Mock<IJwtGenerator>();

        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        var accountService = new AccountService(accountRepositoryMock.Object,
            _mapper, jwtGeneratorMock.Object, userAccessorMock.Object);

        // Act
        var result = accountService.Verify(userId);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(Roles.Client)]
    [InlineData(Roles.Unverified)]
    public void Verify_UserIsNotEmployeeOrManager_ReturnsFalse(string roleName)
    {
        // Arrange
        var accountRepositoryMock = new Mock<IAccountRepository>();
        var jwtGeneratorMock = new Mock<IJwtGenerator>();

        var userId = Guid.NewGuid();
        var userRole = new Role { Name = roleName };
        var user = new User() { Role = userRole };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var accountService = new AccountService(accountRepositoryMock.Object,
            _mapper, jwtGeneratorMock.Object, userAccessorMock.Object);

        // Act
        var result = accountService.Verify(userId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Verify_UserRoleIsNull_ReturnsFalse()
    {
        // Arrange
        var jwtGeneratorMock = new Mock<IJwtGenerator>();

        var userId = Guid.NewGuid();
        var userRole = new Role { Name = Roles.Manager };
        var user = new User() { Role = userRole };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.GetUserRoleName(userId)).Returns<string?>(null);

        var accountService = new AccountService(accountRepositoryMock.Object,
            _mapper, jwtGeneratorMock.Object, userAccessorMock.Object);

        // Act
        var result = accountService.Verify(userId);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(Roles.Client)]
    [InlineData(Roles.Employee)]
    [InlineData(Roles.Manager)]
    public void Verity_UserRoleIsNotUnverified_ReturnsFalse(string userToVerifyRoleName)
    {
        // Arrange
        var jwtGeneratorMock = new Mock<IJwtGenerator>();

        var userId = Guid.NewGuid();
        var userRole = new Role { Name = Roles.Manager };
        var user = new User() { Role = userRole };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.GetUserRoleName(userId)).Returns(userToVerifyRoleName);

        var accountService = new AccountService(accountRepositoryMock.Object,
            _mapper, jwtGeneratorMock.Object, userAccessorMock.Object);

        // Act
        var result = accountService.Verify(userId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Verify_ClientRoleNotFound_ReturnFalse()
    {
        // Arrange
        var jwtGeneratorMock = new Mock<IJwtGenerator>();

        var userId = Guid.NewGuid();
        var userRole = new Role { Name = Roles.Manager };
        var user = new User() { Role = userRole };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.GetUserRoleName(userId)).Returns(Roles.Unverified);
        accountRepositoryMock.Setup(x => x.GetRoleByName(Roles.Client)).Returns<Role?>(null);

        var accountService = new AccountService(accountRepositoryMock.Object,
            _mapper, jwtGeneratorMock.Object, userAccessorMock.Object);

        // Act
        var result = accountService.Verify(userId);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public void Verify_RoleIdNotUpdated_ReturnFalse()
    {
        // Arrange
        var jwtGeneratorMock = new Mock<IJwtGenerator>();

        var userId = Guid.NewGuid();
        var userRole = new Role { Name = Roles.Manager };
        var user = new User() { Role = userRole };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var clientRoleId = Guid.NewGuid();
        var clientRole = new Role { Id = clientRoleId };
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.GetUserRoleName(userId)).Returns(Roles.Unverified);
        accountRepositoryMock.Setup(x => x.GetRoleByName(Roles.Client)).Returns(clientRole);
        accountRepositoryMock.Setup(x => x.UpdateUserRoleId(userId, clientRoleId)).Returns(false);

        var accountService = new AccountService(accountRepositoryMock.Object,
            _mapper, jwtGeneratorMock.Object, userAccessorMock.Object);

        // Act
        var result = accountService.Verify(userId);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(Roles.Employee)]
    [InlineData(Roles.Manager)]
    public void Verify_UserIsEmployeeOrManager_ReturnsTrue(string userRoleName)
    {
        // Arrange
        var jwtGeneratorMock = new Mock<IJwtGenerator>();

        var userId = Guid.NewGuid();
        var userRole = new Role { Name = userRoleName };
        var user = new User() { Role = userRole };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var clientRoleId = Guid.NewGuid();
        var clientRole = new Role { Id = clientRoleId };
        var accountRepositoryMock = new Mock<IAccountRepository>();
        accountRepositoryMock.Setup(x => x.GetUserRoleName(userId)).Returns(Roles.Unverified);
        accountRepositoryMock.Setup(x => x.GetRoleByName(Roles.Client)).Returns(clientRole);
        accountRepositoryMock.Setup(x => x.UpdateUserRoleId(userId, clientRoleId)).Returns(true);

        var accountService = new AccountService(accountRepositoryMock.Object,
            _mapper, jwtGeneratorMock.Object, userAccessorMock.Object);

        // Act
        var result = accountService.Verify(userId);

        // Assert
        Assert.True(result);
    }
}