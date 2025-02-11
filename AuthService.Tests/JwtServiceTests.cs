using AuthService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Xunit;

public class JwtServiceTests
{
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
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

        // Act
        var token = _jwtService.GenerateToken(email);

        // Assert
        Assert.NotNull(token);
        Assert.Contains(".", token); // Проверяем, что токен похож на JWT
    }
}
