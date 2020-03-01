using System;
using DemoAPI.Client;

namespace DemoAPI.ClientAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client started...");
            Console.WriteLine("A - for API client");
            Console.WriteLine("G - for grpc client");
            Console.WriteLine("Q - for quitting");
            var quit = false;
            while (!quit)
            {
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
            }

            Console.WriteLine("Client stopped...");
            Console.ReadKey();
        }

        static void RunGrpcClient()
        {
            Console.WriteLine("Enter forecast quantity...");
            var dataQuantityString = Console.ReadLine();
            Console.WriteLine();

            if (int.TryParse(dataQuantityString, out int result))
            {
                GrpcClient.Run(result).Wait();
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
                APIClient.Run(result).Wait();
            }
            else
            {
                Console.WriteLine("Incorrect data...");
            }
        }

    }
}
