using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SerhatKaya.CheckList
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
                    webBuilder.UseStartup<Startup>().UseUrls(new[] { "http://0.0.0.0:4800" });
                });
    }
}