using Infrastructure.UnitOfWork;

namespace Api.Configuration
{
    public static class UnitOfWorkCfg
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
