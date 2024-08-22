using Application.Interfaces;
using Application.Services;
using Core.Response;

namespace Api.Configuration
{
    public static class ServicesCfg
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMotorcycleService, MotorcycleService>();
            services.AddScoped<IMotorcycleRentalService, MotorcycleRentalService>();
            
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
