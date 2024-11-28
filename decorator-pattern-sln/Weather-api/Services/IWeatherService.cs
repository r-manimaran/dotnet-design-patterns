namespace Weather_api.Services
{
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> GetForecastForecasts(string cityName);
    }
}
