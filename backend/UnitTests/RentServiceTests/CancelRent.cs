using System;
using System.Collections.Generic;
using Application.Constants;
using Application.Core;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTests.RentServiceTests;

public class CancelRent
{
    private readonly Mock<IUserAccessor> _userAccessorMock;
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly RentService _rentService;

    public CancelRent()
    {
        var mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();

        _userAccessorMock = new();
        _rentRepositoryMock = new();

        _rentService = new(mapper, _userAccessorMock.Object, _rentRepositoryMock.Object);
    }

    [Fact]
    public void CancelRent_RentNotFound_ReturnsFalse()
    {
        // Arrange
        var rentId = Guid.NewGuid();

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns<Rent?>(null);

        // Act
        var result = _rentService.CancelRent(rentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CancelRent_RentDepartmentIdNotFound_ReturnsFalse()
    {
        // Arrange
        var userAccessorMock = new Mock<IUserAccessor>();

        var rentId = Guid.NewGuid();
        var rent = new Rent { Id = rentId };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        _rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns<Guid?>(null);

        // Act
        var result = _rentService.CancelRent(rentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CancelRent_UserNotLogged_ReturnsFalse()
    {
        // Arrange
        var rentId = Guid.NewGuid();
        var rent = new Rent { Id = rentId };
        var departmentId = Guid.NewGuid();

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        _rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        // Act
        var result = _rentService.CancelRent(rentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CancelRent_RentDepartmentNotFound_ReturnsFalse()
    {
        // Arrange
        var userRole = new Role { Name = Roles.Client };
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Role = userRole
        };

        var rentId = Guid.NewGuid();
        var rent = new Rent
        {
            Id = rentId,
            ClientId = userId
        };

        var departmentId = Guid.NewGuid();

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        _rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        _rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns<Department?>(null);

        // Act
        var result = _rentService.CancelRent(rentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CancelRent_UserUnverified_ReturnsFalse()
    {
        // Arrange
        var userRole = new Role { Name = Roles.Unverified };
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Role = userRole
        };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rent = new Rent
        {
            Id = rentId,
            ClientId = userId
        };

        var managerId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            Employees = new List<User>(),
            ManagerId = managerId
        };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        _rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        _rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);

        // Act
        var result = _rentService.CancelRent(rentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CancelRent_RentIsNotReserved_ReturnsFalse()
    {
        // Arrange
        var userRole = new Role { Name = Roles.Client };
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Role = userRole
        };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rentStatusId = Guid.NewGuid();
        var rent = new Rent
        {
            Id = rentId,
            ClientId = userId,
            RentStatusId = rentStatusId,
        };

        var managerId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            Employees = new List<User>(),
            ManagerId = managerId
        };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        _rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        _rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(Guid.NewGuid());

        // Act
        var result = _rentService.CancelRent(rentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CancelRent_ArchivedStatusIdNotFound_ReturnsFalse()
    {
        // Arrange
        var userRole = new Role { Name = Roles.Client };
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Role = userRole
        };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rentStatusId = Guid.NewGuid();
        var rent = new Rent
        {
            Id = rentId,
            ClientId = userId,
            RentStatusId = rentStatusId,
        };

        var managerId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            Employees = new List<User>(),
            ManagerId = managerId
        };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        _rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        _rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(rentStatusId);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Archived)).Returns<Guid?>(null);

        // Act
        var result = _rentService.CancelRent(rentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CancelRent_RentNotUpdated_ReturnsFalse()
    {
        // Arrange
        var userRole = new Role { Name = Roles.Client };
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Role = userRole
        };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rentStatusId = Guid.NewGuid();
        var rent = new Rent
        {
            Id = rentId,
            ClientId = userId,
            RentStatusId = rentStatusId,
        };

        var managerId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            Employees = new List<User>(),
            ManagerId = managerId
        };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        _rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        _rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(rentStatusId);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Archived)).Returns(Guid.NewGuid());
        _rentRepositoryMock.Setup(x => x.UpdateRent(rent)).Returns(false);

        // Act
        var result = _rentService.CancelRent(rentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CancelRent_UserIsManager_ReturnsTrue()
    {
        // Arrange
        var userRole = new Role { Name = Roles.Manager };
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Role = userRole
        };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rentStatusId = Guid.NewGuid();
        var rent = new Rent
        {
            Id = rentId,
            ClientId = userId,
            RentStatusId = rentStatusId,
        };

        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            Employees = new List<User>(),
            ManagerId = userId
        };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        _rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        _rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(rentStatusId);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Archived)).Returns(Guid.NewGuid());
        _rentRepositoryMock.Setup(x => x.UpdateRent(rent)).Returns(true);

        // Act
        var result = _rentService.CancelRent(rentId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CancelRent_UserIsEmployee_ReturnsTrue()
    {
        // Arrange
        var userRole = new Role { Name = Roles.Manager };
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Role = userRole
        };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rentStatusId = Guid.NewGuid();
        var rent = new Rent
        {
            Id = rentId,
            ClientId = userId,
            RentStatusId = rentStatusId,
        };

        var managerId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            Employees = new List<User> { new User { Id = userId } },
            ManagerId = managerId
        };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        _rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        _rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(rentStatusId);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Archived)).Returns(Guid.NewGuid());
        _rentRepositoryMock.Setup(x => x.UpdateRent(rent)).Returns(true);

        // Act
        var result = _rentService.CancelRent(rentId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CancelRent_UserIsClient_ReturnsTrue()
    {
        // Arrange
        var userRole = new Role { Name = Roles.Client };
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Role = userRole
        };

        _userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rentStatusId = Guid.NewGuid();
        var rent = new Rent
        {
            Id = rentId,
            ClientId = userId,
            RentStatusId = rentStatusId,
        };

        var managerId = Guid.NewGuid();
        var departmentId = Guid.NewGuid();
        var department = new Department
        {
            Employees = new List<User>(),
            ManagerId = managerId
        };

        _rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        _rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        _rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(rentStatusId);
        _rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Archived)).Returns(Guid.NewGuid());
        _rentRepositoryMock.Setup(x => x.UpdateRent(rent)).Returns(true);

        // Act
        var result = _rentService.CancelRent(rentId);

        // Assert
        Assert.True(result);
    }
}