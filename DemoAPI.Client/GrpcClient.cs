using System;
using System.Threading.Tasks;
using DemoAPI.Common;
using DemoAPI.gRPC;
using Grpc.Core;
using Grpc.Net.Client;

namespace DemoAPI.Client
{
    public static class GrpcClient
    {
        public static async Task GetMultipleForecasts(int quantity)
        {
            var weatherForecastClient = GetGrpcClient();
            var streamingResult = weatherForecastClient.GetForecastInfo(new GetForecastRequest
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

        private static void ShowForecast(ForecastResult result)
        {
            Console.WriteLine($"Date: {result.Date} Temperature: {result.TemperatureC} Summary: {result.Summary} Golfable? {(result.CanYouPlayGolf ? "Yes" : "No")}");
        }

        public static async Task GetForecastForDate(string date)
        {
            var weatherForecastClient = GetGrpcClient();
            var result = await weatherForecastClient.GetForecastForDateInfoAsync(
                new GetForecastForDateRequest { Date = date });
            
            ShowForecast(result);
        }

        private static WeatherForecast.WeatherForecastClient GetGrpcClient()
        {
            var channel = GrpcChannel.ForAddress(Configuration.UrlForClient);
            return new WeatherForecast.WeatherForecastClient(channel);
        }
    }
}