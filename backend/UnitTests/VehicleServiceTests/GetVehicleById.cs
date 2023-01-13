using System;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.VehicleServiceTests
{
    public class GetVehicleById
    {
        private readonly IMapper _mapper;

        public GetVehicleById()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
                .CreateMapper();
        }

        [Fact]
        public void GetVehicleById_DepartmentNotFound_ReturnsNull()
        {
            // Arrange
            var userAccessorMock = new Mock<IUserAccessor>();
            var vehicleId = Guid.NewGuid();
            var departmentRepositoryMock = new Mock<IVehicleRepository>();
            departmentRepositoryMock.Setup(x => x.GetVehicleById(vehicleId)).Returns<Vehicle?>(null);

            var departmentService = new VehicleService(departmentRepositoryMock.Object, _mapper, userAccessorMock.Object);

            // Act
            var result =  departmentService.GetVehicleById(vehicleId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetVehicleById_CorrectRequest_ReturnsResult()
        {
            // Arrange
            var userAccessorMock = new Mock<IUserAccessor>();
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle();
            var departmentRepositoryMock = new Mock<IVehicleRepository>();
            departmentRepositoryMock.Setup(x => x.GetVehicleById(vehicleId)).Returns(vehicle);

            var departmentService = new VehicleService(departmentRepositoryMock.Object, _mapper, userAccessorMock.Object);

            // Act
            var result =  departmentService.GetVehicleById(vehicleId);

            // Assert
            Assert.NotNull(result);
        }
    }
}