using System;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.RentServiceTests;

public class GetRentById
{
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly RentService _rentService;

    public GetRentById()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _userAccessorMock = new();
        _rentRepositoryMock = new();

        _rentService = new(mapper, _userAccessorMock.Object, _rentRepositoryMock.Object);
    }

    [Fact]
    public void GetRentById_UserNotLogged_ReturnsNull()
    {
        // Arrange
        var rentId = Guid.NewGuid();

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        // Act
        var result = _rentService.GetRentById(rentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetRentById_RentNotFound_ReturnsNull()
    {
        // Arrange
        var user = new User();

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        
        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns<Rent?>(null);

        // Act
        var result = _rentService.GetRentById(rentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetRentById_UserIsUnverified_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rent = new Rent
        {
            ClientId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            RenterId = Guid.NewGuid(),
        };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);

        // Act
        var result = _rentService.GetRentById(rentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetRentById_UserIsClient_ReturnsResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rent = new Rent
        {
            ClientId = userId,
            ReceiverId = Guid.NewGuid(),
            RenterId = Guid.NewGuid(),
        };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);

        // Act
        var result = _rentService.GetRentById(rentId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetRentById_UserIsRenter_ReturnsResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rent = new Rent
        {
            ClientId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            RenterId = userId,
        };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);

        // Act
        var result = _rentService.GetRentById(rentId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetRentById_UserIsReceiver_ReturnsResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rent = new Rent
        {
            ClientId = Guid.NewGuid(),
            ReceiverId = userId,
            RenterId = Guid.NewGuid(),
        };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);

        // Act
        var result = _rentService.GetRentById(rentId);

        // Assert
        Assert.NotNull(result);
    }
}