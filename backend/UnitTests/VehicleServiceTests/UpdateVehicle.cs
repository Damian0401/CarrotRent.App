using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly IMapper _mapper;

    public UpdateVehicle()
    {
        _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();
    }

    [Fact]
    public void UpdateVehicle_UserNotLogged_ReturnsFalse()
    {
        // Arrange
        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        var dto = new UpdateVehicleDtoRequest();
        var vehicleId = Guid.NewGuid();

        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.UpdateVehicle(vehicleId, dto);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns<Department?>(null);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.UpdateVehicle(vehicleId, dto);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var managerId = Guid.NewGuid();
        var department = new Department
        {
            Employees = new List<User>(),
            ManagerId = managerId,
        };

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns(department);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.UpdateVehicle(vehicleId, dto);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var department = new Department
        {
            Employees = new List<User>(),
            ManagerId = userId,
        };

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns(department);
        vehicleRepositoryMock.Setup(x => x.UpdateVehicle(vehicleId, It.IsAny<Vehicle>())).Returns(true);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.UpdateVehicle(vehicleId, dto);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var managerId = Guid.NewGuid();
        var employees = new List<User> { user };
        var department = new Department
        {
            Employees = employees,
            ManagerId = managerId,
        };

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetVehicleDepartment(vehicleId)).Returns(department);
        vehicleRepositoryMock.Setup(x => x.UpdateVehicle(vehicleId, It.IsAny<Vehicle>())).Returns(true);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.UpdateVehicle(vehicleId, dto);

        // Assert
        Assert.True(result);
    }
}