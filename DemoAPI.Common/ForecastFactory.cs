using System;
using System.Collections.Generic;
using System.Linq;

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
            var temperature = rng.Next(-20, 55);

            return Enumerable.Range(0, quantity).Select(index => 
            (
                date:  DateTime.Now.AddDays(index).Date.ToShortDateString(),
                summary: Summaries[rng.Next(Summaries.Length)],
                temperatureC: temperature,
                canYouPlayGold: temperature > 20
            ));
        }
    }
}
