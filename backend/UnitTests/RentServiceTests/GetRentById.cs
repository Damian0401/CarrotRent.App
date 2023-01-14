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
    private readonly IMapper _mapper;

    public GetRentById()
    {
        _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();
    }

    [Fact]
    public void GetRentById_UserNotLogged_ReturnsNull()
    {
        // Arrange
        var rentId = Guid.NewGuid();
        var rentRepositoryMock = new Mock<IRentRepository>();

        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetRentById(rentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetRentById_RentNotFound_ReturnsNull()
    {
        // Arrange
        var user = new User();
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns<Rent?>(null);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetRentById(rentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetRentById_UserIsUnverified_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rent = new Rent
        {
            ClientId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            RenterId = Guid.NewGuid(),
        };
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetRentById(rentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetRentById_UserIsClient_ReturnsResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rent = new Rent
        {
            ClientId = userId,
            ReceiverId = Guid.NewGuid(),
            RenterId = Guid.NewGuid(),
        };
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetRentById(rentId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetRentById_UserIsRenter_ReturnsResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rent = new Rent
        {
            ClientId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            RenterId = userId,
        };
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetRentById(rentId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetRentById_UserIsReceiver_ReturnsResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rent = new Rent
        {
            ClientId = Guid.NewGuid(),
            ReceiverId = userId,
            RenterId = Guid.NewGuid(),
        };
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetRentById(rentId);

        // Assert
        Assert.NotNull(result);
    }
}