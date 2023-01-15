using System;
using System.Collections.Generic;
using System.Linq;
using Application.Constants;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.RentServiceTests;

public class GetMyRents
{
    private readonly IMapper _mapper;

    public GetMyRents()
    {
        _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();
    }

    [Fact]
    public void GetMyRents_UserNotLogged_ReturnsNull()
    {
        // Arrange
        var rentRepositoryMock = new Mock<IRentRepository>();

        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetMyRents();

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(Roles.Unverified)]
    [InlineData(Roles.Employee)]
    [InlineData(Roles.Manager)]
    public void GetMyRents_UserNotClient_ReturnsNull(string userRoleName)
    {
        // Arrange
        var rentRepositoryMock = new Mock<IRentRepository>();

        var userRole = new Role { Name = userRoleName };
        var user = new User { Role = userRole };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetMyRents();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetMyRents_CorrectRequest_ReturnsResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRole = new Role { Name = Roles.Client };
        var user = new User
        {
            Id = userId,
            Role = userRole
        };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rents = new List<Rent>();
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetUserRents(userId)).Returns(rents);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetMyRents();

        // Assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(50)]
    public void GetMyRents_CorrectRentNumber_ReturnsResult(int rentNumber)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRole = new Role { Name = Roles.Client };
        var user = new User
        {
            Id = userId,
            Role = userRole
        };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rents = new List<Rent>();
        Enumerable.Range(0, rentNumber).ToList().ForEach(_ => rents.Add(new Rent()));
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetUserRents(userId)).Returns(rents);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetMyRents();

        // Assert
        Assert.Equal(rentNumber, result!.Rents.Count);
    }
}