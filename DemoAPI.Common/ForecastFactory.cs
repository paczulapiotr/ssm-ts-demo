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

        public static IEnumerable<(string date, string summary, int temperatureC, bool canYouPlayGolf)> Create(int quantity)
        {
            var rng = new Random();
            foreach (var index in Enumerable.Range(0, quantity))
            {
                var temperature = rng.Next(-20, 55);
                Task.Delay(300).Wait();
                yield return
                    (
                    date: DateTime.Now.AddDays(index).Date.ToShortDateString(),
                    summary: Summaries[rng.Next(Summaries.Length)],
                    temperatureC: temperature,
                    canYouPlayGolf: temperature > 20
                    );
            }
        }
    }
}
