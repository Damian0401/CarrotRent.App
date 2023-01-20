using System;
using System.Collections.Generic;
using Application.Constants;
using Application.Core;
using Application.Dtos.Rent;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.RentServiceTests;

public class CreateRent
{
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly RentService _rentService;

    public CreateRent()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _userAccessorMock = new();
        _rentRepositoryMock = new();

        _rentService = new(mapper, _userAccessorMock.Object, _rentRepositoryMock.Object);
    }

    [Fact]
    public void CreateRent_UserNotLogged_ReturnsFalse()
    {
        // Arrange
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        var dto = new CreateRentDtoRequest();

        // Act
        var result = _rentService.CreateRent(dto);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(Roles.Unverified)]
    [InlineData(Roles.Employee)]
    [InlineData(Roles.Manager)]
    public void CreateRent_UserIsNotClient_ReturnsFalse(string userRoleName)
    {
        // Arrange
        var dto = new CreateRentDtoRequest();

        var userRole = new Role { Name = userRoleName };
        var user = new User { Role = userRole };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        // Act
        var result = _rentService.CreateRent(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateRent_RentsFoundBetween_ReturnsFalse()
    {
        // Arrange
        var dto = new CreateRentDtoRequest();

        var userRole = new Role { Name = Roles.Client };
        var user = new User { Role = userRole };
        var rents = new List<Rent> { new() };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);
        _rentRepositoryMock.Setup(x =>
            x.GetRentBetweenDates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(rents);

        // Act
        var result = _rentService.CreateRent(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateRent_ReservedRentStatusNotFound_ReturnsFalse()
    {
        // Arrange
        var dto = new CreateRentDtoRequest();

        var userRole = new Role { Name = Roles.Client };
        var user = new User { Role = userRole };
        var rents = new List<Rent>();

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);
        _rentRepositoryMock.Setup(x =>
            x.GetRentBetweenDates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(rents);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns<Guid?>(null);

        // Act
        var result = _rentService.CreateRent(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateRent_RentNotCreated_ReturnFalse()
    {
        // Arrange
        var dto = new CreateRentDtoRequest();

        var userRole = new Role { Name = Roles.Client };
        var user = new User { Role = userRole };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rents = new List<Rent>();
        var reservedRentStatusId = Guid.NewGuid();

        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(reservedRentStatusId);
        _rentRepositoryMock.Setup(x =>
            x.GetRentBetweenDates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(rents);
        _rentRepositoryMock.Setup(x => x.CreateRent(It.IsAny<Rent>())).Returns(false);

        // Act
        var result = _rentService.CreateRent(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateRent_CorrectRequest_ReturnsTrue()
    {
        // Arrange
        var dto = new CreateRentDtoRequest();

        var userRole = new Role { Name = Roles.Client };
        var user = new User { Role = userRole };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var reservedRentStatusId = Guid.NewGuid();
        var rents = new List<Rent>();

        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(reservedRentStatusId);
        _rentRepositoryMock.Setup(x =>
            x.GetRentBetweenDates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(rents);
        _rentRepositoryMock.Setup(x => x.CreateRent(It.IsAny<Rent>())).Returns(true);

        // Act
        var result = _rentService.CreateRent(dto);

        // Assert
        Assert.True(result);
    }
}