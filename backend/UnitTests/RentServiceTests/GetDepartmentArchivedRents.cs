using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.RentServiceTests;

public class GetDepartmentArchivedRents
{
    private readonly IMapper _mapper;

    public GetDepartmentArchivedRents()
    {
        _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();
    }

    [Fact]
    public void GetDepartmentArchivedRents_UserNotLogged_ReturnsNull()
    {
        // Arrange
        var rentRepositoryMock = new Mock<IRentRepository>();
        var departmentId = Guid.NewGuid();

        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetDepartmentArchivedRents(departmentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetDepartmentArchivedRents_DepartmentNotFound_ReturnsNull()
    {
        // Arrange
        var departmentId = Guid.NewGuid();

        var user = new User();
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns<Department?>(null);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetDepartmentArchivedRents(departmentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetDepartmentArchivedRents_UserIsManager_ReturnsResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            ManagerId = userId,
            Employees = new List<User>()
        };

        var rents = new List<Rent>();
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        rentRepositoryMock.Setup(x => x.GetDepartmentArchivedRents(departmentId)).Returns(rents);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetDepartmentArchivedRents(departmentId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetDepartmentArchivedRents_UserIsEmployee_ReturnsResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var managerId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            ManagerId = managerId,
            Employees = new List<User> { new User { Id = userId } }
        };

        var rents = new List<Rent>();
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        rentRepositoryMock.Setup(x => x.GetDepartmentArchivedRents(departmentId)).Returns(rents);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetDepartmentArchivedRents(departmentId);

        // Assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(50)]
    public void GetDepartmentArchivedRents_CorrectRequest_ReturnsResponse(int rentNumber)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            ManagerId = userId,
            Employees = new List<User>()
        };

        var rents = new List<Rent>();
        Enumerable.Range(0, rentNumber).ToList().ForEach(_ => rents.Add(new Rent()));

        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        rentRepositoryMock.Setup(x => x.GetDepartmentArchivedRents(departmentId)).Returns(rents);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.GetDepartmentArchivedRents(departmentId);

        // Assert
        Assert.Equal(rentNumber, result!.Rents.Count);
    }
}