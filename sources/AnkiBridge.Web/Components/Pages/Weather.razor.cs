using Microsoft.AspNetCore.Components;

namespace AnkiBridge.Web.Components.Pages;

public partial class Weather
{
    [Inject]
    private WeatherApiClient WeatherApi { get; set; } = default!;

    private IQueryable<WeatherForecast>? forecasts;

    protected override async Task OnInitializedAsync()
    {
        forecasts = await WeatherApi.GetWeatherAsync();
    }
}
