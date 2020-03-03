using System;
using System.Threading.Tasks;
using DemoAPI.Common;
using DemoAPI.gRPC;
using Grpc.Core;

namespace DemoAPI.Server.Services
{
	public class WeatherForecastService : WeatherForecast.WeatherForecastBase
	{
		public WeatherForecastService()
		{

		}

		public override async Task GetForecastInfo(GetForecastRequest request, IServerStreamWriter<ForecastResult> responseStream, ServerCallContext context)
		{
			foreach (var forecast in ForecastFactory.CreateMultiple(request.ForecastDaysQuantity))
			{
				await responseStream.WriteAsync(new ForecastResult
				{
					Date = forecast.date,
					TemperatureC = forecast.temperatureC,
					Summary = forecast.summary,
					CanYouPlayGolf = forecast.canYouPlayGolf
				});
			}
		}

		public override Task<ForecastResult> GetForecastForDateInfo(GetForecastForDateRequest request, ServerCallContext context)
		{
			var parsedDate = DateParserHelper.Parse(request.Date);

			var forecast = ForecastFactory.Create(parsedDate);

			return Task.FromResult(new ForecastResult
			{
				Date = forecast.date,
				TemperatureC = forecast.temperatureC,
				Summary = forecast.summary,
				CanYouPlayGolf = forecast.canYouPlayGolf
			});
		}
	}
}
