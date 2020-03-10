using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DemoAPI.Client;
using DemoAPI.Common;

namespace DemoAPI.ClientAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client started...");
            var quit = false;
            while (!quit)
            {
                DisplayMenuOptions();
                var keyInfo = Console.ReadKey();
                Console.WriteLine();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Q:
                        quit = true;
                        break;

                    case ConsoleKey.D1:
                        RESTFutureForecast();
                        break;

                    case ConsoleKey.D2:
                        GrpcFutureForecast();
                        break;

                    case ConsoleKey.D3:
                        RESTForecastForDay();
                        break;

                    case ConsoleKey.D4:
                        GrpcForecastForDay();
                        break;

                    case ConsoleKey.D5:
                        GrpcPostForecast();
                        break;

                    case ConsoleKey.D6:
                        GrpcBidirectionalForecast();
                        break;

                    default:
                        break;
                }
                Console.ReadKey();
            }

            Console.WriteLine("Client stopped...");
            Console.ReadKey();
        }

        private static void DisplayMenuOptions()
        {
            Console.Clear();
            Console.WriteLine("1 - (REST) Forecast for several days");
            Console.WriteLine("2 - (gRPC) Forecast for several days");
            Console.WriteLine("3 - (REST) Forecast for a specific day");
            Console.WriteLine("4 - (gRPC) Forecast for a specific day");
            Console.WriteLine("...");
            Console.WriteLine("5 - (gRPC) Client streaming");
            Console.WriteLine("6 - (gRPC) Bidirectional streaming");
            Console.WriteLine("Q - for quitting");
        }

        static void GrpcFutureForecast() => FutureForecastBase(GrpcClient.GetForecast);
        static void RESTForecastForDay() => ForecastForDayBase(RESTClient.GetForecastForDate);
        static void GrpcPostForecast() => FutureForecastBase(GrpcClient.PostForecast);
        static void GrpcBidirectionalForecast() => FutureForecastBase(GrpcClient.BidirectionalForecast);

        static void RESTFutureForecast() => FutureForecastBase(RESTClient.GetMultipleForecasts);

        static void GrpcForecastForDay() => ForecastForDayBase(GrpcClient.GetForecastForDate);

        private static void FutureForecastBase(Func<int, Task> clientAction)
        {
            Console.WriteLine("Enter forecast quantity...");
            var dataQuantityString = Console.ReadLine();
            Console.WriteLine();

            if (int.TryParse(dataQuantityString, out int result))
            {
                MeasureTime(clientAction(result)).Wait();
            }
            else
            {
                Console.WriteLine("Incorrect data...");
            }
        }

        private static void ForecastForDayBase(Func<string, Task> clientAction)
        {
            Console.WriteLine($"Enter forecast date with format {Configuration.DateFormat}...");
            var date = Console.ReadLine();
            Console.WriteLine();
            MeasureTime(clientAction(date)).Wait();
        }

        static async Task MeasureTime(Task action)
        {
            var timer = Stopwatch.StartNew();
            await action;
            timer.Stop();
            Console.WriteLine($"Time elapsed: {timer.ElapsedMilliseconds}ms");
        }

    }
}
