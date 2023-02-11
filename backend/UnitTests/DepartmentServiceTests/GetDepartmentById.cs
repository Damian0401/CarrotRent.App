using System;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.DepartmentServiceTests;

public class GetDepartmentById
{
    private readonly Mock<IDepartmentRepository> _departmentRepositoryMock;
    private readonly DepartmentService _departmentService;

    public GetDepartmentById()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _departmentRepositoryMock = new();

        _departmentService = new(_departmentRepositoryMock.Object, mapper);
    }

    [Fact]
    public void GetDepartmentById_DepartmentNotFound_ReturnsNull()
    {
        // Arrange
        var departmentId = Guid.NewGuid();
        _departmentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns<Department?>(null);

        // Act
        var result =  _departmentService.GetDepartmentById(departmentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetDepartmentById_CorrectRequest_ReturnsResult()
    {
        // Arrange
        var departmentId = Guid.NewGuid();
        var department = new Department();
        _departmentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);

        // Act
        var result = _departmentService.GetDepartmentById(departmentId);

        // Assert
        Assert.NotNull(result);
    }
}