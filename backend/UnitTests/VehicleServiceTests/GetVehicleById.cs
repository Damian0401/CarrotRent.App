using System;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.VehicleServiceTests;

public class GetVehicleById
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly VehicleService _vehicleService;

    public GetVehicleById()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _vehicleRepositoryMock = new();
        _userAccessorMock = new();

        _vehicleService = new(_vehicleRepositoryMock.Object, mapper, _userAccessorMock.Object);
    }

    [Fact]
    public void GetVehicleById_DepartmentNotFound_ReturnsNull()
    {
        // Arrange
        var userAccessorMock = new Mock<IUserAccessor>();
        var vehicleId = Guid.NewGuid();
        _vehicleRepositoryMock.Setup(x => x.GetVehicleById(vehicleId)).Returns<Vehicle?>(null);

        // Act
        var result = _vehicleService.GetVehicleById(vehicleId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetVehicleById_CorrectRequest_ReturnsResponse()
    {
        // Arrange
        var userAccessorMock = new Mock<IUserAccessor>();
        var vehicleId = Guid.NewGuid();
        var vehicle = new Vehicle();
        _vehicleRepositoryMock.Setup(x => x.GetVehicleById(vehicleId)).Returns(vehicle);

        // Act
        var result = _vehicleService.GetVehicleById(vehicleId);

        // Assert
        Assert.NotNull(result);
    }
}