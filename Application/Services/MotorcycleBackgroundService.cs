using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using AutoMapper;

using Application.Interfaces;
using Application.Validations;
using Application.DTO.Application.Motorcycle;
using Application.DTO.Configuration;
using Core.Entities;
using Infrastructure.UnitOfWork;

namespace Application.Services
{
    public class MotorcycleBackgroundService : IMotorcycleBackgroundService
    {
        private readonly ILogger<MotorcycleBackgroundService> logger;
        private readonly IOptions<KafkaSettings> kafkaSettings;
        private readonly IMotorcycleAdapterService motorcycleAdapterService;
        private readonly MotorcycleAddRequestValidation addRequestValidation = new();
        IServiceProvider serviceProvider;

        public MotorcycleBackgroundService(
            ILogger<MotorcycleBackgroundService> logger,
            IOptions<KafkaSettings> kafkaSettings,
                IMapper mapper,
                IMotorcycleAdapterService motorcycleAdapterService,
                IServiceProvider serviceProvider
        )
        {
            this.logger = logger;
            this.kafkaSettings = kafkaSettings;
            this.motorcycleAdapterService = motorcycleAdapterService;
            this.serviceProvider = serviceProvider;

        }

        public async Task Event()
        {
            using var scope = serviceProvider.CreateScope();

            IUnitOfWork unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            IMapper mapper = scope.ServiceProvider.GetService<IMapper>();

            List<MotorcycleAddRequestDTO>? requestsToProcess = await this.ReadMotorcyclesFromKafka(int.Parse(kafkaSettings.Value.DefaultLimit));
            List<MotorcycleModel> motorcycleModels = mapper.Map<List<MotorcycleModel>>(requestsToProcess);

            if (!motorcycleModels.Any())
                return;

            foreach (var item in motorcycleModels)
                await unitOfWork.Motorcycle.Insert(item);

            await unitOfWork.CommitAsync();

            motorcycleModels.Where(moto => moto.Year.Equals(20244)).ToList()
                .ForEach(moto =>
            {
                this.logger.LogInformation($"Motorcycle with Year 2024 saved into the database: {moto.Plate} - {moto.Model}");
            });
        }

        private async Task<List<MotorcycleAddRequestDTO>> ReadMotorcyclesFromKafka(int limit)
        {
            try
            {
                List<MotorcycleAddRequestDTO> dataFromKafka = new List<MotorcycleAddRequestDTO>();

                for (int i = 0; i < limit; i++)
                {
                    var kafkaDataCheck = this.motorcycleAdapterService.GetMessageAsync();

                    if (kafkaDataCheck is null)
                        break;

                    dataFromKafka.Add(JsonSerializer.Deserialize<MotorcycleAddRequestDTO>(kafkaDataCheck));
                }

                return dataFromKafka;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
