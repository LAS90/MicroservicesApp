using AuthService.Data;
using AuthService.Models;
using AuthService.DTOs;
using AuthService.Interfaces; // Добавили интерфейс
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using AuthService.Repositories;
using AuthService.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserService> _userServiceMock; // Теперь мокируем сервис
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userServiceMock = new Mock<IUserService>(); // Мокируем интерфейс
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task RegisterUser_SavesUserWithHashedPassword()
    {
        // Arrange
        var dto = new RegisterUserDto { Email = "test@example.com", Password = "securepassword" };

        _userRepositoryMock.Setup(repo => repo.UserExists(dto.Email)).ReturnsAsync(false);
        _userRepositoryMock.Setup(repo => repo.AddUser(It.IsAny<User>())).Returns(Task.CompletedTask);
        _userServiceMock.Setup(s => s.HashPassword(dto.Password)).Returns("hashedpassword");

        // Act
        var result = await _userService.RegisterUser(dto);

        // Assert
        Assert.True(result);
        _userRepositoryMock.Verify(repo => repo.AddUser(It.Is<User>(u =>
            u.Email == dto.Email && u.PasswordHash != dto.Password)), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_WithExistingEmail_ReturnsFalse()
    {
        // Arrange
        var dto = new RegisterUserDto { Email = "test@example.com", Password = "securepassword" };
        _userRepositoryMock.Setup(repo => repo.UserExists(dto.Email)).ReturnsAsync(true);

        // Act
        var result = await _userService.RegisterUser(dto);

        // Assert
        Assert.False(result);
        _userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task GetUserByEmail_ReturnsUserIfExists()
    {
        // Arrange
        var user = new User { Email = "test@example.com", PasswordHash = "hashedpassword" };
        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(user.Email)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByEmail(user.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result!.Email);
    }

    [Fact]
    public async Task GetUserByEmail_ReturnsNullIfUserDoesNotExist()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserByEmail("nonexistent@example.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void VerifyPassword_CorrectPassword_ReturnsTrue()
    {
        // Arrange
        var password = "securepassword";
        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = _userService.HashPassword(password) 
        };

        // Act
        var result = _userService.VerifyPassword(user, password);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_IncorrectPassword_ReturnsFalse()
    {
        // Arrange
        var password = "securepassword";
        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = _userService.HashPassword(password)
        };

        // Act
        var result = _userService.VerifyPassword(user, "wrongpassword");

        // Assert
        Assert.False(result);
    }
}
