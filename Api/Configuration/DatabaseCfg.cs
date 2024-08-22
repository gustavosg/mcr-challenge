using Core;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Api.Configuration
{
    public static class DatabaseCfg
    {
        public static IServiceCollection SetupDatabase(this IServiceCollection services, string? connection)
        {
            services.AddDbContext<AppDbContext>(
                opts =>
                    opts.UseNpgsql(connection,
                    optsBuilder =>
                    {
                        optsBuilder.MigrationsAssembly(Assembly.Load(Constants.INFRASTRUCTURE_PROJECT_NAME).FullName);
                        optsBuilder.SetPostgresVersion(13, 0);
                    }
                )
            );
            return services;
        }
    }
}
