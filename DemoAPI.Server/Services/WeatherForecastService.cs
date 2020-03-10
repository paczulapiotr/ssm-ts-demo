using System;
using System.Threading.Tasks;
using DemoAPI.Common;
using DemoAPI.gRPC;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DemoAPI.Server.Services
{
	public class WeatherForecastService : WeatherForecast.WeatherForecastBase
	{
		public WeatherForecastService()
		{
		}

		public override Task<ForecastResult> ForecastInfo(GetForecastForDateRequest request, ServerCallContext context)
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

		public override async Task ForecastInfoServerStreaming(GetForecastRequest request, IServerStreamWriter<ForecastResult> responseStream, ServerCallContext context)
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

		public override async Task<Empty> ForecastInfoClientStreaming(IAsyncStreamReader<PostForecastRequest> requestStream, ServerCallContext context)
		{
			while (await requestStream.MoveNext())
			{
				var current = requestStream.Current;
				Console.WriteLine($"Date: {current.Date}, Temperature: {current.TemperatureC} C, Summary: {current.Summary}, Golfable: {current.CanYouPlayGolf}");
			}

			return new Empty();
		}

		public override async Task ForecastInfoBidirectionalStreaming(IAsyncStreamReader<GetForecastForDateRequest> requestStream, IServerStreamWriter<ForecastResult> responseStream, ServerCallContext context)
		{
			while (await requestStream.MoveNext())
			{
				var current = requestStream.Current;
				Console.WriteLine($"Request for date: {current.Date}");

				var (date, summary, temperatureC, canYouPlayGolf) = ForecastFactory.Create(DateParserHelper.Parse(current.Date));
				await responseStream.WriteAsync(
					new ForecastResult
					{
						Date = date,
						TemperatureC = temperatureC,
						CanYouPlayGolf = canYouPlayGolf,
						Summary = summary
					});
			}
		}
	}
}
