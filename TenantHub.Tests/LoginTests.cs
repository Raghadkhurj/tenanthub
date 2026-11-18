using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TenantHub.Data;
using TenantHub.Models;

namespace TenantHub.Tests;

public class LoginTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public LoginTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    // Helper: creates a test user in "innovation-grid" with a unique email
    private async Task<string> CreateTestUserAsync(string password)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var tenant = db.Tenants.First(t => t.Slug == "innovation-grid");
        var email = $"test_{Guid.NewGuid()}@test.com";

        db.Users.Add(new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            TenantId = tenant.Id
        });
        await db.SaveChangesAsync();

        return email;
    }

    [Fact]
    public async Task Login_WithCorrectData_ReturnsOk()
    {
        // Arrange
        var email = await CreateTestUserAsync("Test@123");

        // Act
        var response = await _client.PostAsJsonAsync("/auth/user/login",
            new { email, password = "Test@123", companySlug = "innovation-grid" });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task Login_WithrongPassword_ReturnUnAuthorized()
    {
        // Arrange
        var email = await CreateTestUserAsync("test@123");
        // Act
        var response = await _client.PostAsJsonAsync("/auth/user/login",
            new { email, password = "Wrong@123", companySlug = "innovation-grid" });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
   [Fact]
   public async Task Login_WithrongTenant_ReturnUnAuthorized()
    {
        // Arrange
        var email = await CreateTestUserAsync("test@123");
        // Act
        var response = await _client.PostAsJsonAsync("/auth/user/login",
            new { email, password = "test@123", companySlug = "orange-company" });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    } 
}