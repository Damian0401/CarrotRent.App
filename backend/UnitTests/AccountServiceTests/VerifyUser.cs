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

public class VerifyUser
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly Mock<IJwtGenerator> _JwtGeneratorMock;
    private readonly AccountService _accountService;
    
    public VerifyUser()
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
    public void Verify_UserNotLogged_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        // Act
        var result = _accountService.VerifyUser(userId);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(Roles.Client)]
    [InlineData(Roles.Unverified)]
    public void Verify_UserIsNotEmployeeOrManager_ReturnsFalse(string roleName)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRole = new Role { Name = roleName };
        var user = new User() { Role = userRole };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        // Act
        var result = _accountService.VerifyUser(userId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Verify_UserRoleIsNull_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRole = new Role { Name = Roles.Manager };
        var user = new User() { Role = userRole };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        _accountRepositoryMock.Setup(x => x.GetUserRoleName(userId)).Returns<string?>(null);

        // Act
        var result = _accountService.VerifyUser(userId);

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
        var userId = Guid.NewGuid();
        var userRole = new Role { Name = Roles.Manager };
        var user = new User() { Role = userRole };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        _accountRepositoryMock.Setup(x => x.GetUserRoleName(userId)).Returns(userToVerifyRoleName);

        // Act
        var result = _accountService.VerifyUser(userId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Verify_ClientRoleNotFound_ReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRole = new Role { Name = Roles.Manager };
        var user = new User() { Role = userRole };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        _accountRepositoryMock.Setup(x => x.GetUserRoleName(userId)).Returns(Roles.Unverified);
        _accountRepositoryMock.Setup(x => x.GetRoleByName(Roles.Client)).Returns<Role?>(null);

        // Act
        var result = _accountService.VerifyUser(userId);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public void Verify_RoleIdNotUpdated_ReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRole = new Role { Name = Roles.Manager };
        var user = new User() { Role = userRole };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var clientRoleId = Guid.NewGuid();
        var clientRole = new Role { Id = clientRoleId };
        _accountRepositoryMock.Setup(x => x.GetUserRoleName(userId)).Returns(Roles.Unverified);
        _accountRepositoryMock.Setup(x => x.GetRoleByName(Roles.Client)).Returns(clientRole);
        _accountRepositoryMock.Setup(x => x.UpdateUserRoleId(userId, clientRoleId)).Returns(false);

        // Act
        var result = _accountService.VerifyUser(userId);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(Roles.Employee)]
    [InlineData(Roles.Manager)]
    public void Verify_UserIsEmployeeOrManager_ReturnsTrue(string userRoleName)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRole = new Role { Name = userRoleName };
        var user = new User() { Role = userRole };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var clientRoleId = Guid.NewGuid();
        var clientRole = new Role { Id = clientRoleId };
        _accountRepositoryMock.Setup(x => x.GetUserRoleName(userId)).Returns(Roles.Unverified);
        _accountRepositoryMock.Setup(x => x.GetRoleByName(Roles.Client)).Returns(clientRole);
        _accountRepositoryMock.Setup(x => x.UpdateUserRoleId(userId, clientRoleId)).Returns(true);

        // Act
        var result = _accountService.VerifyUser(userId);

        // Assert
        Assert.True(result);
    }
}