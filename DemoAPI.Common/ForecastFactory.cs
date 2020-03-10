using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Common
{
    public static class ForecastFactory
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public static async IAsyncEnumerable<(string date, string summary, int temperatureC, bool canYouPlayGolf)> CreateMultipleAsync(int quantity)
        {
            foreach (var index in Enumerable.Range(0, quantity))
            {
                yield return await CreateAsync(DateTime.Today.AddDays(index));
            }
        }

        public static async Task<(string date, string summary, int temperatureC, bool canYouPlayGolf)> CreateAsync(DateTime date)
        {
            var rng = new Random();
            var temperature = rng.Next(-20, 55);
            await Task.Delay(1000);
            return
            (
            date: date.ToShortDateString(),
            summary: Summaries[rng.Next(Summaries.Length)],
            temperatureC: temperature,
            canYouPlayGolf: temperature > 20
            );
        }

    }
}
