using System;
using System.Threading.Tasks;
using DemoAPI.gRPC;
using Grpc.Core;
using Grpc.Net.Client;

namespace DemoAPI.Client
{
    public static class GrpcClient
    {
        public static async Task Run(int quantity)
        {
            var channel = GrpcChannel.ForAddress("");
            var weatherForecastClient = new WeatherForecast.WeatherForecastClient(channel);
            var streamingResult = weatherForecastClient.GetForecastInfo(new GetForecastRequest
            {
                ForecastDaysQuantity = quantity
            });
            var responseStream = streamingResult.ResponseStream;

            while (await responseStream.MoveNext())
            {
                var result = responseStream.Current;
                Console.WriteLine($"Date: {result.Date} Temperature: {result.TemperatureC} Summary: {result.Summary} Golfable? {(result.CanYouPlayGolf ? "Yes" : "No")}");
            }
        }
    }
}