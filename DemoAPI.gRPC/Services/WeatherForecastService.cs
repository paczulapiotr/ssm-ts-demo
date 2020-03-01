using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Common;
using Grpc.Core;

namespace DemoAPI.gRPC
{

	public class WeatherForecastService : WeatherForecast.WeatherForecastBase
	{
		public WeatherForecastService()
		{

		}

		public override async Task GetForecastInfo(GetForecastRequest request, IServerStreamWriter<ForecastResult> responseStream, ServerCallContext context)
		{
			foreach (var forecast in ForecastFactory.Create(request.ForecastDaysQuantity))
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
	}
}
