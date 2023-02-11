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
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly RentService _rentService;

    public GetMyRents()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _userAccessorMock = new();
        _rentRepositoryMock = new();

        _rentService = new(mapper, _userAccessorMock.Object, _rentRepositoryMock.Object);
    }

    [Fact]
    public void GetMyRents_UserNotLogged_ReturnsNull()
    {
        // Arrange
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        // Act
        var result = _rentService.GetMyRents();

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
        var userRole = new Role { Name = userRoleName };
        var user = new User { Role = userRole };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        // Act
        var result = _rentService.GetMyRents();

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

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rents = new List<Rent>();

        _rentRepositoryMock.Setup(x => x.GetUserArchivedRents(userId)).Returns(rents);

        // Act
        var result = _rentService.GetMyRents();

        // Assert
        Assert.NotNull(result);
    }
}