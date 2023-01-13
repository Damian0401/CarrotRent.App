using System;
using Domain.Models;
using Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace UnitTests.JwtGeneratorTests;

public class CreateToken
{
    [Theory]
    [InlineData("super secret key 1", "super secret key 2")]
    [InlineData("1234567890987654321", "0987654321234567890")]
    [InlineData("c9fb75fb-6d57-4343-bee8-0506d87d7716", "05d3c707-af4e-42e7-be43-4faa1453c076")]
    public void CreateToken_DiffrentKeys_ReturnsDiffrentTokens(string key1, string key2) 
    {
        // Arrange
        var expireDate = DateTime.Now.AddDays(1);
        var role = new Role() { Name = "UserRole" };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = "UserLogin",
            Role = role
        };

        var configurationMock1 = new Mock<IConfiguration>();
        configurationMock1.Setup(c => c["Jwt:Key"]).Returns(key1);
        var jwtGenerator1 = new JwtGenerator(configurationMock1.Object);

        var configurationMock2 = new Mock<IConfiguration>();
        configurationMock2.Setup(c => c["Jwt:Key"]).Returns(key2);
        var jwtGenerator2 = new JwtGenerator(configurationMock2.Object);

        // Act
        var token1 = jwtGenerator1.CreateToken(user, expireDate);
        var token2 = jwtGenerator2.CreateToken(user, expireDate);

        // Assert
        Assert.NotEqual(token1, token2);
    }

    [Theory]
    [InlineData("UserLogin1", "UserLogin2")]
    [InlineData("1234567890987654321", "0987654321234567890")]
    [InlineData("c9fb75fb-6d57-4343-bee8-0506d87d7716", "05d3c707-af4e-42e7-be43-4faa1453c076")]
    public void CreateToken_DiffrentLogins_ReturnsDiffrentTokens(string login1, string login2) 
    {
        // Arrange
        var expireDate = DateTime.Now.AddDays(1);
        var role = new Role() { Name = "UserRole" };

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["Jwt:Key"]).Returns("super secret key");
        var jwtGenerator = new JwtGenerator(configurationMock.Object);

        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Login = login1,
            Role = role
        };

        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Login = login2,
            Role = role
        };


        // Act
        var token1 = jwtGenerator.CreateToken(user1, expireDate);
        var token2 = jwtGenerator.CreateToken(user2, expireDate);

        // Assert
        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void CreateToken_SameData_ReturnsSameTokens() 
    {
        // Arrange
        var expireDate = DateTime.Now.AddDays(1);
        var role = new Role() { Name = "UserRole" };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = "UserLogin1",
            Role = role
        };

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["Jwt:Key"]).Returns("super secret key");
        var jwtGenerator = new JwtGenerator(configurationMock.Object);

        // Act
        var token1 = jwtGenerator.CreateToken(user, expireDate);
        var token2 = jwtGenerator.CreateToken(user, expireDate);

        // Assert
        Assert.Equal(token1, token2);
    }
}