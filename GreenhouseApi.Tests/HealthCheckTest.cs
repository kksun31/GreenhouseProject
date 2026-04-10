using Microsoft.AspNetCore.Mvc.Testing;

namespace GreenhouseApi.Tests;

public class HealthCheckTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthCheckTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        // Arrange: Создаем виртуального клиента (браузер)
        var client = _factory.CreateClient();

        // Act: Стучимся по адресу /health
        var response = await client.GetAsync("/health");
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert: Проверяем, что всё прошло успешно и сервер ответил "Healthy"
        response.EnsureSuccessStatusCode();
        Assert.Equal("palka", responseString);
    }
}
