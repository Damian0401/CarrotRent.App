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
    private readonly IMapper _mapper;

    public DeleteVehicle()
    {
        _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();
    }

    [Fact]
    public void DeleteVehicle_UserNotLogged_ReturnsFalse()
    {
        // Arrange
        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        var vehicleId = Guid.NewGuid();

        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.DeleteVehicle(vehicleId);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void DeleteVehicle_DepartmentNotFound_ReturnsFalse()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();

        var user = new User();
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns<Department?>(null);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.DeleteVehicle(vehicleId);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var managerId = Guid.NewGuid();
        var department = new Department
        {
            ManagerId = managerId,
        };

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns(department);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.DeleteVehicle(vehicleId);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var department = new Department { ManagerId = userId };

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns(department);
        vehicleRepositoryMock.Setup(x => x.DeleteVehicle(vehicleId)).Returns(true);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.DeleteVehicle(vehicleId);

        // Assert
        Assert.True(result);
    }
}