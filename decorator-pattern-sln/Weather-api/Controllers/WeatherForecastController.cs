using Microsoft.AspNetCore.Mvc;
using Weather_api.Services;

namespace Weather_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet(Name = "GetWeatherForecast/{cityName}")]
        public IEnumerable<WeatherForecast> Get(string cityName)
        {
            var result = _weatherService.GetForecastForecasts(cityName);
            return result;
        }
    }
}
