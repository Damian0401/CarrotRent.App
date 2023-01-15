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
    private readonly IMapper _mapper;

    public CancelRent()
    {
        _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()))
            .CreateMapper();
    }

    [Fact]
    public void CancelRent_RentNotFound_ReturnsFalse()
    {
        // Arrange
        var userAccessorMock = new Mock<IUserAccessor>();

        var rentId = Guid.NewGuid();
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns<Rent?>(null);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CancelRent(rentId);

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
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns<Guid?>(null);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CancelRent(rentId);

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
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);

        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns<User?>(null);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CancelRent(rentId);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

        var rentId = Guid.NewGuid();
        var rent = new Rent
        {
            Id = rentId,
            ClientId = userId
        };

        var departmentId = Guid.NewGuid();
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns<Department?>(null);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CancelRent(rentId);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CancelRent(rentId);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(Guid.NewGuid());

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CancelRent(rentId);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(rentStatusId);
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Archived)).Returns<Guid?>(null);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CancelRent(rentId);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(rentStatusId);
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Archived)).Returns(Guid.NewGuid());
        rentRepositoryMock.Setup(x => x.UpdateRent(rent)).Returns(false);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CancelRent(rentId);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(rentStatusId);
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Archived)).Returns(Guid.NewGuid());
        rentRepositoryMock.Setup(x => x.UpdateRent(rent)).Returns(true);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CancelRent(rentId);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(rentStatusId);
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Archived)).Returns(Guid.NewGuid());
        rentRepositoryMock.Setup(x => x.UpdateRent(rent)).Returns(true);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CancelRent(rentId);

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
        var userAccessorMock = new Mock<IUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentlyLoggedUser()).Returns(user);

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
        var rentRepositoryMock = new Mock<IRentRepository>();
        rentRepositoryMock.Setup(x => x.GetRentById(rentId)).Returns(rent);
        rentRepositoryMock.Setup(x => x.GetRentDepartmentId(rentId)).Returns(departmentId);
        rentRepositoryMock.Setup(x => x.GetDepartmentById(departmentId)).Returns(department);
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Reserved)).Returns(rentStatusId);
        rentRepositoryMock.Setup(x => x.GetRentStatusIdByName(RentStatuses.Archived)).Returns(Guid.NewGuid());
        rentRepositoryMock.Setup(x => x.UpdateRent(rent)).Returns(true);

        var rentService = new RentService(_mapper, userAccessorMock.Object, rentRepositoryMock.Object);

        // Act
        var result = rentService.CancelRent(rentId);

        // Assert
        Assert.True(result);
    }
}