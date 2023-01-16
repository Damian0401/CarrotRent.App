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

public class DeleteVehicle
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly VehicleService _vehicleService;

    public DeleteVehicle()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _vehicleRepositoryMock = new();
        _userAccessorMock = new();

        _vehicleService = new(_vehicleRepositoryMock.Object, mapper, _userAccessorMock.Object);
    }

    [Fact]
    public void DeleteVehicle_UserNotLogged_ReturnsFalse()
    {
        // Arrange
        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        var vehicleId = Guid.NewGuid();

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        // Act
        var result = _vehicleService.DeleteVehicle(vehicleId);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void DeleteVehicle_DepartmentNotFound_ReturnsFalse()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var user = new User();

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);
        _vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns<Department?>(null);

        // Act
        var result = _vehicleService.DeleteVehicle(vehicleId);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void DeleteVehicle_UserIsNotManager_ReturnsFalse()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var managerId = Guid.NewGuid();
        var department = new Department { ManagerId = managerId };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);
        _vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns(department);

        // Act
        var result = _vehicleService.DeleteVehicle(vehicleId);

        // Assert
        Assert.False(result);
    }
        
    [Fact]
    public void DeleteVehicle_CorrectRequest_ReturnsTrue()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var department = new Department { ManagerId = userId };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);
        _vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns(department);
        _vehicleRepositoryMock.Setup(x => x.DeleteVehicle(vehicleId)).Returns(true);

        // Act
        var result = _vehicleService.DeleteVehicle(vehicleId);

        // Assert
        Assert.True(result);
    }
}