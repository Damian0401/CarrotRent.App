using System;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.DepartmentServiceTests
{
    public class GetDepartmentById
    {
        private readonly IMapper _mapper;

        public GetDepartmentById()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
                .CreateMapper();
        }

        [Fact]
        public void GetDepartmentById_DepartmentNotFound_ReturnsNull()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var departmentRepositoryMock = new Mock<IDepartmentRepository>();
            departmentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns<Department?>(null);

            var departmentService = new DepartmentService(departmentRepositoryMock.Object, _mapper);

            // Act
            var result =  departmentService.GetDepartmentById(departmentId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetDepartmentById_CorrectRequest_ReturnsResult()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var department = new Department();
            var departmentRepositoryMock = new Mock<IDepartmentRepository>();
            departmentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);

            var departmentService = new DepartmentService(departmentRepositoryMock.Object, _mapper);

            // Act
            var result =  departmentService.GetDepartmentById(departmentId);

            // Assert
            Assert.NotNull(result);
        }
    }
}