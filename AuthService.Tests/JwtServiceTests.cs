using AuthService.Interfaces; // Добавили интерфейс
using Microsoft.Extensions.Configuration;
using Xunit;
using Moq;
using AuthService.Services;

public class JwtServiceTests
{
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly IJwtService _jwtService;

    public JwtServiceTests()
    {
        _jwtServiceMock = new Mock<IJwtService>();

        // Создаём in-memory конфигурацию
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Jwt:Key", "SuperSecretKeyThatIsAtLeast32CharactersLong!" }
        }!;

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _jwtService = new JwtService(configuration);
    }

    [Fact]
    public void GenerateToken_Returns_Valid_Token()
    {
        // Arrange
        var email = "test@example.com";
        _jwtServiceMock.Setup(s => s.GenerateToken(email)).Returns("mock-token");

        // Act
        var token = _jwtService.GenerateToken(email);

        // Assert
        Assert.NotNull(token);
        Assert.Contains(".", token); // Проверяем, что токен похож на JWT
    }
}
