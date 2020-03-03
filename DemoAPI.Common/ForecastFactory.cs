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

        public static IEnumerable<(string date, string summary, int temperatureC, bool canYouPlayGolf)> CreateMultiple(int quantity)
        {
            foreach (var index in Enumerable.Range(0, quantity))
            {
                yield return Create(DateTime.Today.AddDays(index));
            }
        }

        public static (string date, string summary, int temperatureC, bool canYouPlayGolf) Create(DateTime date)
        {
            var rng = new Random();
            var temperature = rng.Next(-20, 55);
            Task.Delay(300).Wait();
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
