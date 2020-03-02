using DemoAPI.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
namespace GRPC.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel(opts => opts.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http2));
                    var host = Configuration.UrlForServer ?? "https://+:5000";
                    webBuilder.UseUrls(host);
                });
    }
}
