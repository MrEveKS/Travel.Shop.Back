using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Travel.Shop.Back.Services
{
    public static class MigrationUtils
    {
        /// <summary>
        /// Initialize Database
        /// </summary>
        /// <typeparam name="T">Database Context</typeparam>
        /// <param name="host">Web Host</param>
        public static IHost InitializeDatabase<T>(this IHost host)
            where T : DbContext
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var provider = serviceScope.ServiceProvider;
                
                try
                {
                    var context = provider.GetRequiredService<T>();

                    context.Database.Migrate();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return host;
        }
    }
}
