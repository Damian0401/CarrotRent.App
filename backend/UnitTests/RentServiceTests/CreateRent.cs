using System;
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
    private readonly IMapper _mapper;

    public CreateRent()
    {
        _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();
    }

    [Fact]
    public void CreateRent_UserNotLogged_ReturnsFalse()
    {
        // Arrange
        var rentRepositoryMock = new Mock<IRentRepository>();

        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        var dto = new CreateRentDtoRequest();

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CreateRent(dto);

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
        var rentRepositoryMock = new Mock<IRentRepository>();
        var dto = new CreateRentDtoRequest();

        var userRole = new Role { Name = userRoleName };
        var user = new User { Role = userRole };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CreateRent(dto);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns<Guid?>(null);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CreateRent(dto);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var reservedRentStatusId = Guid.NewGuid();
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(reservedRentStatusId);
        rentRepositoryMock.Setup(x => x.CreateRent(It.IsAny<Rent>())).Returns(false);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CreateRent(dto);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var reservedRentStatusId = Guid.NewGuid();
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(reservedRentStatusId);
        rentRepositoryMock.Setup(x => x.CreateRent(It.IsAny<Rent>())).Returns(true);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CreateRent(dto);

        // Assert
        Assert.True(result);
    }
}