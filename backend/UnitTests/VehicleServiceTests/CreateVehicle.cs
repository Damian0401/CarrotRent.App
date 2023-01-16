using System;
using Application.Core;
using Application.Dtos.Vehicle;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.VehicleServiceTests;

public class CreateVehicle
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly VehicleService _vehicleService;

    public CreateVehicle()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _vehicleRepositoryMock = new();
        _userAccessorMock = new();

        _vehicleService = new(_vehicleRepositoryMock.Object, mapper, _userAccessorMock.Object);
    }

    [Fact]
    public void CreateVehicle_UserNotLogged_ReturnsFalse()
    {
        // Arrange
        var dto = new CreateVehicleDtoRequest();

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        // Act
        var result = _vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_DepartmentNotFound_ReturnsFalse()
    {
        // Arrange
        var user = new User();
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        _vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns<Department?>(null);

        var dto = new CreateVehicleDtoRequest { DepartmentId = departmentId };

        // Act
        var result = _vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_UserNotDepartmentManager_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var managerId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var department = new Department { ManagerId = managerId };
        _vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);

        var dto = new CreateVehicleDtoRequest { DepartmentId = departmentId };

        // Act
        var result = _vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_VinNotAvailable_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        var department = new Department { ManagerId = userId };

        var vin = "RequestVin";
        var dto = new CreateVehicleDtoRequest
        {
            DepartmentId = departmentId,
            Vin = vin
        };

        _vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(false);

        // Act
        var result = _vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_RegistrationNotAvailable_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        var department = new Department { ManagerId = userId };

        var vin = "RequestVin";
        var registration = "RequestRegistration";
        var dto = new CreateVehicleDtoRequest
        {
            DepartmentId = departmentId,
            Vin = vin,
            Registration = registration
        };

        _vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(true);
        _vehicleRepositoryMock.Setup(x => x.IsRegistrationAvailable(registration)).Returns(false);

        // Act
        var result = _vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_FuelNotFound_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        var department = new Department { ManagerId = userId };

        var vin = "RequestVin";
        var registration = "RequestRegistration";
        var fuelId = Guid.NewGuid();
        var dto = new CreateVehicleDtoRequest
        {
            DepartmentId = departmentId,
            Vin = vin,
            Registration = registration,
            FuelId = fuelId
        };

        _vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(true);
        _vehicleRepositoryMock.Setup(x => x.IsRegistrationAvailable(registration)).Returns(true);
        _vehicleRepositoryMock.Setup(x => x.GetFuelById(fuelId)).Returns<Fuel?>(null);

        // Act
        var result = _vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_ModelNotFound_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        var department = new Department { ManagerId = userId };

        var vin = "RequestVin";
        var registration = "RequestRegistration";
        var fuelId = Guid.NewGuid();
        var fuel = new Fuel();
        var modelId = Guid.NewGuid();
        var dto = new CreateVehicleDtoRequest
        {
            DepartmentId = departmentId,
            Vin = vin,
            Registration = registration,
            FuelId = fuelId,
            ModelId = modelId
        };

        _vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(true);
        _vehicleRepositoryMock.Setup(x => x.IsRegistrationAvailable(registration)).Returns(true);
        _vehicleRepositoryMock.Setup(x => x.GetFuelById(fuelId)).Returns(fuel);
        _vehicleRepositoryMock.Setup(x => x.GetModelById(modelId)).Returns<Model?>(null);

        // Act
        var result = _vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_VehicleNotCreated_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        var department = new Department { ManagerId = userId };

        var vin = "RequestVin";
        var registration = "RequestRegistration";
        var fuelId = Guid.NewGuid();
        var fuel = new Fuel();
        var modelId = Guid.NewGuid();
        var model = new Model();
        var dto = new CreateVehicleDtoRequest
        {
            DepartmentId = departmentId,
            Vin = vin,
            Registration = registration,
            FuelId = fuelId,
            ModelId = modelId
        };

        _vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(true);
        _vehicleRepositoryMock.Setup(x => x.IsRegistrationAvailable(registration)).Returns(true);
        _vehicleRepositoryMock.Setup(x => x.GetFuelById(fuelId)).Returns(fuel);
        _vehicleRepositoryMock.Setup(x => x.GetModelById(modelId)).Returns(model);
        _vehicleRepositoryMock.Setup(x => x.CreateVehicle(It.IsAny<Vehicle>())).Returns(false);

        // Act
        var result = _vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_CorrectRequest_ReturnsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        var department = new Department { ManagerId = userId };

        var vin = "RequestVin";
        var registration = "RequestRegistration";
        var fuelId = Guid.NewGuid();
        var fuel = new Fuel();
        var modelId = Guid.NewGuid();
        var model = new Model();
        var dto = new CreateVehicleDtoRequest
        {
            DepartmentId = departmentId,
            Vin = vin,
            Registration = registration,
            FuelId = fuelId,
            ModelId = modelId
        };

        _vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(true);
        _vehicleRepositoryMock.Setup(x => x.IsRegistrationAvailable(registration)).Returns(true);
        _vehicleRepositoryMock.Setup(x => x.GetFuelById(fuelId)).Returns(fuel);
        _vehicleRepositoryMock.Setup(x => x.GetModelById(modelId)).Returns(model);
        _vehicleRepositoryMock.Setup(x => x.CreateVehicle(It.IsAny<Vehicle>())).Returns(true);

        // Act
        var result = _vehicleService.CreateVehicle(dto);

        // Assert
        Assert.True(result);
    }
}