using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GRPC.API
{
    public class Program
    {

        public static string HOSTING_URL
            => Environment.GetEnvironmentVariable("HOSTING_URL");

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    var host = "http://localhost:5000";
                    webBuilder.UseUrls(host);
                });
    }
}
