using System;

namespace GRPC.API.Controllers
{
    public class WeatherForecast
    {
        public DateTime Date { get; internal set; }
        public string Summary { get; internal set; }
        public int TemperatureC { get; internal set; }
    }
}