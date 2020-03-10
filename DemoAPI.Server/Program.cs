using System;
using DemoAPI.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GRPC.API
{
    public class Program
    {
        public static string CERT_PASSWORD => Environment.GetEnvironmentVariable("certPassword") ?? "1234";
        public static string USE_CERT => Environment.GetEnvironmentVariable("useCert") ?? string.Empty;
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    #region docker settings
                    //    webBuilder.UseKestrel(opts =>
                    //    {
                    //        opts.Listen(IPAddress.Any, Configuration.AppPort,
                    //        o =>
                    //            {
                    //                if (USE_CERT.ToLower().Equals("yes"))
                    //                {
                    //                    o.UseHttps("cert.pfx", CERT_PASSWORD);
                    //                }
                    //            });
                    //        opts.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http2);

                    //    });
                    //    var host = "https://+:" + Configuration.AppPort;
                    #endregion
                    var host = "https://localhost:" + Configuration.AppPort;
                    webBuilder.UseUrls(host);

                });
    }
}
