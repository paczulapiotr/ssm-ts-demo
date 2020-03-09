using DemoAPI.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace GRPC.API
{
    public class Program
    {
        public static string CERT_PASSWORD => Environment.GetEnvironmentVariable("certPassword") ?? "1234";
        public static string APP_PORT => Environment.GetEnvironmentVariable("appPort") ?? Configuration.AppPort.ToString();
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
                        opts.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http2);
                        opts.ConfigureHttpsDefaults(o => o.ServerCertificate = new X509Certificate2("cert.pfx", CERT_PASSWORD));

                    });
                    var host = "https://+:" + APP_PORT;
                    webBuilder.UseUrls(host);
                });
    }
}
