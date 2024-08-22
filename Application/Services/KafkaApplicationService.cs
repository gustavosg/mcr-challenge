using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using AutoMapper;

using Application.Interfaces;

namespace Application.Services
{
    public class KafkaApplicationService : IHostedService, IDisposable
    {
        private readonly IMotorcycleBackgroundService motorcycleBackgroundService;
        private readonly IMapper mapper;

        private readonly IHostApplicationLifetime hostApplicationLifetime;
        private Timer timer;
        private readonly ILogger<KafkaApplicationService> logger;
        

        public KafkaApplicationService(
            IMotorcycleBackgroundService motorcycleBackgroundService,
            IHostApplicationLifetime hostApplicationLifetime,
            ILogger<KafkaApplicationService> logger
        )
        {
            this.motorcycleBackgroundService = motorcycleBackgroundService;

            this.hostApplicationLifetime = hostApplicationLifetime;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting hosted service.");
            
            timer = new Timer(EventStart, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void EventStart(object? state)
        {
            this.motorcycleBackgroundService.Event();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping hosted service.");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.timer.Dispose();
        }
    }
}
