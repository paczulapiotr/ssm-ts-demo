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

		public override async Task<ForecastResult> ForecastInfo(GetForecastForDateRequest request, ServerCallContext context)
		{
			var parsedDate = DateParserHelper.Parse(request.Date);

			var forecast = await ForecastFactory.CreateAsync(parsedDate);

			return new ForecastResult
			{
				Date = forecast.date,
				TemperatureC = forecast.temperatureC,
				Summary = forecast.summary,
				CanYouPlayGolf = forecast.canYouPlayGolf
			};
		}

		public override async Task ForecastInfoServerStreaming(GetForecastRequest request, IServerStreamWriter<ForecastResult> responseStream, ServerCallContext context)
		{
			await foreach (var forecast in ForecastFactory.CreateMultipleAsync(request.ForecastDaysQuantity))
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

				var (date, summary, temperatureC, canYouPlayGolf) = await ForecastFactory.CreateAsync(DateParserHelper.Parse(current.Date));
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

		public override Task SpamForecastInfoBidirectionalStreaming(IAsyncStreamReader<PostForecastRequest> requestStream, IServerStreamWriter<ForecastResult> responseStream, ServerCallContext context)
		{
			var writeTask = WriteStream(responseStream);
			var readTask = ReadStream(requestStream);

			return Task.WhenAll(writeTask, readTask);
		}

		private async Task ReadStream(IAsyncStreamReader<PostForecastRequest> responseStream)
		{
			while (await responseStream.MoveNext())
			{
				ShowForecast(responseStream.Current);
			}
		}

		private async Task WriteStream(IServerStreamWriter<ForecastResult> requestStream)
		{
			var forecasts = ForecastFactory.CreateMultipleAsync(10);
			await foreach (var f in forecasts)
			{
				await requestStream.WriteAsync(new ForecastResult
				{
					Date = f.date,
					TemperatureC = f.temperatureC,
					Summary = f.summary,
					CanYouPlayGolf = f.canYouPlayGolf
				});
			}
		}

		private void ShowForecast(PostForecastRequest result)
		{
			Console.WriteLine($"Date: {result.Date} Temperature: {result.TemperatureC} Summary: {result.Summary} Golfable? {(result.CanYouPlayGolf ? "Yes" : "No")}");
		}
	}
}
