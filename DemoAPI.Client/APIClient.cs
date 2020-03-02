using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DemoAPI.Common;
using DemoAPI.Common.Models;
using Newtonsoft.Json;

namespace DemoAPI.Client
{
    public class APIClient
    {
        public static async Task Run(int quantity)
        {

            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Get, $"{Configuration.UrlForClient}/WeatherForecast/{quantity}")
            {
                Version = new Version(2, 0)
            };

            var result = await client.SendAsync(req);
            if (result.IsSuccessStatusCode)
            {

                var forecasts = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(await result.Content.ReadAsStringAsync());
                foreach (var f in forecasts)
                {
                    Console.WriteLine($"Date: {f.Date} Temperature: {f.TemperatureC} Summary: {f.Summary} Golfable? {(f.CanYouPlayGolf ? "Yes" : "No")}");
                }
            }
        }
    }
}
