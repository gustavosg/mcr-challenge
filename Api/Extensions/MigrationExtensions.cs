using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AppDbContext appDbContext = scope.ServiceProvider.GetService<AppDbContext>();

            appDbContext.Database.Migrate();
        }
    }
}
