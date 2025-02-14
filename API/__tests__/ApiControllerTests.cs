using API.Source;
using API.Source.Db;
using API.Source.Guard;
using API.Source.Strategy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace API.__tests__;

public class ApiControllerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly ApiKeyStrategy _strategy;

    public ApiControllerTests()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["Jwt:Key"]).Returns("your-secret-key");
        configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("your-issuer");
        configurationMock.Setup(x => x["Jwt:Audience"]).Returns("your-audience");
        configurationMock.Setup(x => x["Jwt:TTL"]).Returns("1");

        TokenManager tokenManager = new(configurationMock.Object);
        _authorizationServiceMock = new Mock<IAuthorizationService>();

        _strategy = new ApiKeyStrategy(_userManagerMock.Object, tokenManager);
        _listUserStrategy = new ListUsersStrategy(_userManagerMock.Object);
    }

    [Fact]
    public async Task ApiKey_ShouldReturnApiKey_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequest { Email = "test@example.com", Password = "password123" };
        var user = new ApplicationUser { Email = request.Email };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock
            .Setup(x => x.CheckPasswordAsync(user, request.Password))
            .ReturnsAsync(true);

        // Act
        var result = await _strategy.HandleAsync(request);

        // Assert
        Assert.True(result.Success);
        Assert.False(string.IsNullOrEmpty(result.Data));
    }

    [Fact]
    public async Task ApiKey_ShouldReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "password123",
        };
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser)null);

        // Act
        var result = await _strategy.HandleAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid Credentials", result.Error);
    }

    [Fact]
    public async Task ApiKey_ShouldReturnError_WhenPasswordIsIncorrect()
    {
        // Arrange
        var request = new LoginRequest { Email = "test@example.com", Password = "wrongpassword" };
        var user = new ApplicationUser { Email = request.Email };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock
            .Setup(x => x.CheckPasswordAsync(user, request.Password))
            .ReturnsAsync(false);

        // Act
        var result = await _strategy.HandleAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid Credentials", result.Error);
    }
}
