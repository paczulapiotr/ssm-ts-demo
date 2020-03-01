using System;
using System.Diagnostics;
using DemoAPI.Client;

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

                    case ConsoleKey.G:
                        RunGrpcClient();
                        break;

                    case ConsoleKey.A:
                        RunAPIClient();
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
            Console.WriteLine("A - for API client");
            Console.WriteLine("G - for grpc client");
            Console.WriteLine("Q - for quitting");
        }

        static void RunGrpcClient()
        {
            Console.WriteLine("Enter forecast quantity...");
            var dataQuantityString = Console.ReadLine();
            Console.WriteLine();

            if (int.TryParse(dataQuantityString, out int result))
            {
                MeasureTime(()=>GrpcClient.Run(result).Wait());
            }
            else
            {
                Console.WriteLine("Incorrect data...");
            }
        }

        static void RunAPIClient()
        {
            Console.WriteLine("Enter forecast quantity...");
            var dataQuantityString = Console.ReadLine();
            Console.WriteLine();

            if (int.TryParse(dataQuantityString, out int result))
            {
                MeasureTime(()=>APIClient.Run(result).Wait());
            }
            else
            {
                Console.WriteLine("Incorrect data...");
            }
        }

        static void MeasureTime(Action action)
        {
            var timer = Stopwatch.StartNew();
            action();
            timer.Stop();
            Console.WriteLine($"Time elapsed: {timer.ElapsedMilliseconds}ms");
        }

    }
}
