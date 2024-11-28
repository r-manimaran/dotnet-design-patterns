
using Microsoft.Extensions.Caching.Memory;

namespace Weather_api.Services
{
    public class CachedWeatherService : IWeatherService
    {
        private readonly IWeatherService _weatherService;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CachedWeatherService> _logger;

        public CachedWeatherService(IWeatherService weatherService,
                                    IMemoryCache memoryCache,
                                    ILogger<CachedWeatherService> logger)
        {
            _weatherService = weatherService;
            _memoryCache = memoryCache;
            _logger = logger;
        }       

        public IEnumerable<WeatherForecast> GetForecastForecasts(string cityName)
        {
            string cacheKey = $"weather-{cityName.ToLower()}";
            return _memoryCache.GetOrCreate(
                                cacheKey,
                                entry =>
                                {
                                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                                    _logger.LogInformation($"Fetching Weather data for city:{cityName} from backend");
                                    return _weatherService.GetForecastForecasts(cityName);
                                });
        }
    }
}
