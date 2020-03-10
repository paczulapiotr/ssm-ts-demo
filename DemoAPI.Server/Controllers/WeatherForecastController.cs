using System.Collections.Generic;
using System.Threading.Tasks;
using DemoAPI.Common;
using DemoAPI.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GRPC.API.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class WeatherForecastController : ControllerBase
    {


        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{quantity}")]
        public async IAsyncEnumerable<WeatherForecast> Get(int quantity)
        {
            await foreach (var f in ForecastFactory.CreateMultipleAsync(quantity))
            {
                yield return new WeatherForecast
                {
                    Date = f.date,
                    TemperatureC = f.temperatureC,
                    Summary = f.summary,
                    CanYouPlayGolf = f.canYouPlayGolf
                };
            }
        }


        [HttpGet]
        public async Task<WeatherForecast> GetForDate(string date)
        {
            var parsedDate = DateParserHelper.Parse(date);

            var forecast = await ForecastFactory.CreateAsync(parsedDate);

            return new WeatherForecast
            {
                Date = forecast.date,
                TemperatureC = forecast.temperatureC,
                Summary = forecast.summary,
                CanYouPlayGolf = forecast.canYouPlayGolf
            };
        }
    }
}
