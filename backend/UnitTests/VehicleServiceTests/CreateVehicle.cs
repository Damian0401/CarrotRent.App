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
    private readonly IMapper _mapper;

    public CreateVehicle()
    {
        _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();
    }

    [Fact]
    public void CreateVehicle_UserNotLogged_ReturnsFalse()
    {
        // Arrange
        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        var dto = new CreateVehicleDtoRequest();

        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_DepartmentNotFound_ReturnsFalse()
    {
        // Arrange
        var user = new User();
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns<Department?>(null);

        var dto = new CreateVehicleDtoRequest { DepartmentId = departmentId };

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_UserNotDepartmentManager_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var managerId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var department = new Department { ManagerId = managerId };
        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);

        var dto = new CreateVehicleDtoRequest { DepartmentId = departmentId };

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_VinNotAvailable_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var departmentId = Guid.NewGuid();
        var department = new Department { ManagerId = userId };

        var vin = "RequestVin";
        var dto = new CreateVehicleDtoRequest
        {
            DepartmentId = departmentId,
            Vin = vin
        };

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(false);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_RegistrationNotAvailable_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(true);
        vehicleRepositoryMock.Setup(x => x.IsRegistrationAvailable(registration)).Returns(false);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_FuelNotFound_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(true);
        vehicleRepositoryMock.Setup(x => x.IsRegistrationAvailable(registration)).Returns(true);
        vehicleRepositoryMock.Setup(x => x.GetFuelById(fuelId)).Returns<Fuel?>(null);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_ModelNotFound_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(true);
        vehicleRepositoryMock.Setup(x => x.IsRegistrationAvailable(registration)).Returns(true);
        vehicleRepositoryMock.Setup(x => x.GetFuelById(fuelId)).Returns(fuel);
        vehicleRepositoryMock.Setup(x => x.GetModelById(modelId)).Returns<Model?>(null);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_VehicleNotCreated_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(true);
        vehicleRepositoryMock.Setup(x => x.IsRegistrationAvailable(registration)).Returns(true);
        vehicleRepositoryMock.Setup(x => x.GetFuelById(fuelId)).Returns(fuel);
        vehicleRepositoryMock.Setup(x => x.GetModelById(modelId)).Returns(model);
        vehicleRepositoryMock.Setup(x => x.CreateVehicle(It.IsAny<Vehicle>())).Returns(false);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.CreateVehicle(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateVehicle_CorrectRequest_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        vehicleRepositoryMock.Setup(x => x.IsVinAvailable(vin)).Returns(true);
        vehicleRepositoryMock.Setup(x => x.IsRegistrationAvailable(registration)).Returns(true);
        vehicleRepositoryMock.Setup(x => x.GetFuelById(fuelId)).Returns(fuel);
        vehicleRepositoryMock.Setup(x => x.GetModelById(modelId)).Returns(model);
        vehicleRepositoryMock.Setup(x => x.CreateVehicle(It.IsAny<Vehicle>())).Returns(true);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, _mapper, userAccessorMock.Object);

        // Act
        var result = vehicleService.CreateVehicle(dto);

        // Assert
        Assert.True(result);
    }
}