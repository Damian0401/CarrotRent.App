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

public class GetDepartmentRents
{
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly RentService _rentService;

    public GetDepartmentRents()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _userAccessorMock = new();
        _rentRepositoryMock = new();

        _rentService = new(mapper, _userAccessorMock.Object, _rentRepositoryMock.Object);
    }

    [Fact]
    public void GetDepartmentRents_UserNotLogged_ReturnsNull()
    {
        // Arrange
        var departmentId = Guid.NewGuid();

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        // Act
        var result = _rentService.GetDepartmentRents(departmentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetDepartmentRents_DepartmentNotFound_ReturnsNull()
    {
        // Arrange
        var departmentId = Guid.NewGuid();

        var user = new User();

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        _rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns<Department?>(null);

        // Act
        var result = _rentService.GetDepartmentRents(departmentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetDepartmentRents_UserIsManager_ReturnsResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            ManagerId = userId,
            Employees = new List<User>()
        };

        var rents = new List<Rent>();

        _rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _rentRepositoryMock.Setup(x => x.GetDepartmentRents(departmentId)).Returns(rents);

        // Act
        var result = _rentService.GetDepartmentRents(departmentId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetDepartmentRents_UserIsEmployee_ReturnsResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var managerId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            ManagerId = managerId,
            Employees = new List<User> { new User { Id = userId } }
        };

        var rents = new List<Rent>();

        _rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _rentRepositoryMock.Setup(x => x.GetDepartmentRents(departmentId)).Returns(rents);

        // Act
        var result = _rentService.GetDepartmentRents(departmentId);

        // Assert
        Assert.NotNull(result);
    }
}