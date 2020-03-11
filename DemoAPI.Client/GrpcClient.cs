using System;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Common;
using DemoAPI.gRPC;
using Grpc.Core;
using Grpc.Net.Client;

namespace DemoAPI.Client
{
    public static class GrpcClient
    {
        public static async Task GetForecast(int quantity)
        {
            var weatherForecastClient = GetGrpcClient();
            var streamingResult = weatherForecastClient.ForecastInfoServerStreaming(
                new GetForecastRequest
                {
                    ForecastDaysQuantity = quantity
                });
            var responseStream = streamingResult.ResponseStream;

            while (await responseStream.MoveNext())
            {
                var result = responseStream.Current;
                ShowForecast(result);
            }
        }

        public static async Task PostForecast(int quantity)
        {
            var weatherForecastClient = GetGrpcClient();
            using (var call = weatherForecastClient.ForecastInfoClientStreaming())
            {
                var forecasts = ForecastFactory.CreateMultipleAsync(quantity);
                var stream = call.RequestStream;
                await foreach (var f in forecasts)
                {
                    await stream.WriteAsync(new PostForecastRequest
                    {
                        Date = f.date,
                        TemperatureC = f.temperatureC,
                        Summary = f.summary,
                        CanYouPlayGolf = f.canYouPlayGolf
                    });
                }
                await stream.CompleteAsync();
            }
        }

        public static async Task BidirectionalForecast(int quantity)
        {
            var client = GetGrpcClient();
            using (var call = client.ForecastInfoBidirectionalStreaming())
            {
                var requestStream = call.RequestStream;
                var responseStream = call.ResponseStream;

                foreach (var index in Enumerable.Range(0, quantity))
                {
                    var date = DateTime.Today.AddDays(index).ToString("dd/MM/yyyy");
                    await requestStream.WriteAsync(new GetForecastForDateRequest { Date = date });

                    if (await responseStream.MoveNext())
                    {
                        var f = responseStream.Current;
                        Console.WriteLine($"Date: {f.Date}, Temperature: {f.TemperatureC}, Summary: {f.Summary}, Golfable: {f.CanYouPlayGolf}");
                    }

                }

                await requestStream.CompleteAsync();
                Console.WriteLine($"Bidirectional stream closed");
            }
        }

        public static async Task GetForecastForDate(string date)
        {
            var weatherForecastClient = GetGrpcClient();
            var result = await weatherForecastClient.ForecastInfoAsync(
                new GetForecastForDateRequest { Date = date });

            ShowForecast(result);
        }

        public static async Task SpamForecasts()
        {
            var weatherForecastClient = GetGrpcClient();
            using (var call = weatherForecastClient.SpamForecastInfoBidirectionalStreaming())
            {
                var requestStream = call.RequestStream;
                var responseStream = call.ResponseStream;
                var readTask = ReadStream(responseStream);
                var writeTask = WriteStream(requestStream);

                await Task.WhenAll(writeTask, readTask);
            }
        }

        private static async Task ReadStream(IAsyncStreamReader<ForecastResult> responseStream)
        {
            while (await responseStream.MoveNext())
            {
                ShowForecast(responseStream.Current);
            }
        }

        private static async Task WriteStream(IClientStreamWriter<PostForecastRequest> requestStream)
        {
            await foreach (var f in ForecastFactory.CreateMultipleAsync(10))
            {
                await requestStream.WriteAsync(new PostForecastRequest
                {
                    Date = f.date,
                    TemperatureC = f.temperatureC,
                    Summary = f.summary,
                    CanYouPlayGolf = f.canYouPlayGolf
                });
            }
            await requestStream.CompleteAsync();
        }

        private static void ShowForecast(ForecastResult result)
        {
            Console.WriteLine($"Date: {result.Date} Temperature: {result.TemperatureC} Summary: {result.Summary} Golfable? {(result.CanYouPlayGolf ? "Yes" : "No")}");
        }

        private static WeatherForecast.WeatherForecastClient GetGrpcClient()
        {
            var channel = GrpcChannel.ForAddress(Configuration.UrlForClient);
            return new WeatherForecast.WeatherForecastClient(channel);
        }
    }
}