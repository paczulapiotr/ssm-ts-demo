using DemoAPI.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;

namespace GRPC.API
{
    public class Program
    {
        public static string CERT_PASSWORD => Environment.GetEnvironmentVariable("certPassword") ?? "1234";
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel(opts =>
                    {
                        opts.Listen(IPAddress.Any, Configuration.AppPort,
                        o =>
                            {
                                o.UseHttps("cert.pfx", CERT_PASSWORD);
                            });
                        opts.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http2);

                    });
                    var host = "https://+:" + Configuration.AppPort;
                    webBuilder.UseUrls(host);
                });
    }
}
