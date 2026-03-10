using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebAPI.Tests;

public class WeatherForecastEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public WeatherForecastEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccessAndFiveItems()
    {
        var response = await _client.GetAsync("/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var forecast = await response.Content.ReadFromJsonAsync<List<WeatherForecastDto>>();
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Count);
    }

    [Fact]
    public async Task GetWeatherForecast_ComputesTemperatureFFromTemperatureC()
    {
        var forecast = await _client.GetFromJsonAsync<List<WeatherForecastDto>>("/weatherforecast");

        Assert.NotNull(forecast);
        Assert.NotEmpty(forecast);

        foreach (var item in forecast)
        {
            var expectedTemperatureF = 32 + (int)(item.TemperatureC / 0.5556);
            Assert.Equal(expectedTemperatureF, item.TemperatureF);
        }
    }

    private sealed record WeatherForecastDto(DateOnly Date, int TemperatureC, string? Summary, int TemperatureF);
}
