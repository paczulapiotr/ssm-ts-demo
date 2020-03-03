using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DemoAPI.Common;
using DemoAPI.Common.Models;
using Newtonsoft.Json;

namespace DemoAPI.Client
{
    public class RESTClient
    {
        public static async Task GetMultipleForecasts(int quantity)
        {
            var forecasts = await RequestWrapper<List<WeatherForecast>>(
                $"{Configuration.UrlForClient}/WeatherForecast/{quantity}");

            foreach (var f in forecasts)
            {
                ShowForecast(f);
            }
        }

        public static async Task GetForecastForDate(string date)
        {
            var forecast = await RequestWrapper<WeatherForecast>(
                $"{Configuration.UrlForClient}/WeatherForecast?date={date}");

            ShowForecast(forecast);
        }

        private static async Task<T> RequestWrapper<T>(string url) where T : class, new()
        {
            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Version = new Version(2, 0)
            };

            var result = await client.SendAsync(req);

            return result.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync())
                : null;
        }

        private static void ShowForecast(WeatherForecast f)
        {
            Console.WriteLine($"Date: {f.Date} Temperature: {f.TemperatureC} Summary: {f.Summary} Golfable? {(f.CanYouPlayGolf ? "Yes" : "No")}");
        }
    }
}
