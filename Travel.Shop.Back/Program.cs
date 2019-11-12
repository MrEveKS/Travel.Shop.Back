using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Travel.Shop.Back.Data;
using Travel.Shop.Back.Services;

namespace Travel.Shop.Back
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .InitializeDatabase<BaseDbContext>()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
