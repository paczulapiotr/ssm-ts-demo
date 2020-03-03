using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        public Task<IEnumerable<WeatherForecast>> Get(int quantity)
        {
            var rng = new Random();
            var temperature = rng.Next(-20, 55);

            return Task.FromResult(ForecastFactory.CreateMultiple(quantity).Select(f =>
            {
                return new WeatherForecast
                {
                    Date = f.date,
                    TemperatureC = f.temperatureC,
                    Summary = f.summary,
                    CanYouPlayGolf = f.canYouPlayGolf
                };
            }));
        }


        [HttpGet]
        public Task<WeatherForecast> GetForDate(string date)
        {
            var parsedDate = DateParserHelper.Parse(date);

            var forecast = ForecastFactory.Create(parsedDate);

            return Task.FromResult(
                new WeatherForecast
                {
                    Date = forecast.date,
                    TemperatureC = forecast.temperatureC,
                    Summary = forecast.summary,
                    CanYouPlayGolf = forecast.canYouPlayGolf
                });
        }
    }
}
