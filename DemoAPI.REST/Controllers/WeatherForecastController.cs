using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Common;
using DemoAPI.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GRPC.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

            return Task.FromResult(ForecastFactory.Create(quantity).Select(f =>
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
    }
}
