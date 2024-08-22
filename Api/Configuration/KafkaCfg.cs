using Application.Interfaces;
using Application.Services;

namespace Api.Configuration
{
    public static class KafkaCfg
    {
        public static IServiceCollection AddKafka(this IServiceCollection services)
        {
            services.AddSingleton<IKafkaConnector, KafkaConnector>();
            services.AddSingleton<IMotorcycleAdapterService, MotorcycleAdapterService>();
            services.AddSingleton<IMotorcycleBackgroundService, MotorcycleBackgroundService>();

            return services;
        }
    }
}
