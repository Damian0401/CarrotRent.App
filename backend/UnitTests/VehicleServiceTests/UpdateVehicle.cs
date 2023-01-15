using System;
using System.Collections.Generic;
using Application.Core;
using Application.Dtos.Vehicle;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.VehicleServiceTests;

public class UpdateVehicle
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly VehicleService _vehicleService;

    public UpdateVehicle()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _vehicleRepositoryMock = new();
        _userAccessorMock = new();

        _vehicleService = new(_vehicleRepositoryMock.Object, mapper, _userAccessorMock.Object);
    }

    [Fact]
    public void UpdateVehicle_UserNotLogged_ReturnsFalse()
    {
        // Arrange
        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        var dto = new UpdateVehicleDtoRequest();
        var vehicleId = Guid.NewGuid();

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        // Act
        var result = _vehicleService.UpdateVehicle(vehicleId, dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void UpdateVehicle_DepartmentNotFound_ReturnsFalse()
    {
        // Arrange
        var dto = new UpdateVehicleDtoRequest();
        var vehicleId = Guid.NewGuid();

        var user = new User();
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        _vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns<Department?>(null);

        // Act
        var result = _vehicleService.UpdateVehicle(vehicleId, dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void UpdateVehicle_UserIsNotManagerOrEmployee_ReturnsFalse()
    {
        // Arrange
        var dto = new UpdateVehicleDtoRequest();
        var vehicleId = Guid.NewGuid();

        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var managerId = Guid.NewGuid();
        var department = new Department
        {
            Employees = new List<User>(),
            ManagerId = managerId,
        };

        _vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns(department);

        // Act
        var result = _vehicleService.UpdateVehicle(vehicleId, dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void UpdateVehicle_UserIsManager_ReturnsTrue()
    {
        // Arrange
        var dto = new UpdateVehicleDtoRequest();
        var vehicleId = Guid.NewGuid();

        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var department = new Department
        {
            Employees = new List<User>(),
            ManagerId = userId,
        };

        _vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns(department);
        _vehicleRepositoryMock.Setup(x => x.UpdateVehicle(vehicleId, It.IsAny<Vehicle>())).Returns(true);

        // Act
        var result = _vehicleService.UpdateVehicle(vehicleId, dto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void UpdateVehicle_UserIsEmployee_ReturnsTrue()
    {
        // Arrange
        var dto = new UpdateVehicleDtoRequest();
        var vehicleId = Guid.NewGuid();

        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var managerId = Guid.NewGuid();
        var employees = new List<User> { user };
        var department = new Department
        {
            Employees = employees,
            ManagerId = managerId,
        };

        _vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns(department);
        _vehicleRepositoryMock.Setup(x => x.UpdateVehicle(vehicleId, It.IsAny<Vehicle>())).Returns(true);

        // Act
        var result = _vehicleService.UpdateVehicle(vehicleId, dto);

        // Assert
        Assert.True(result);
    }
}