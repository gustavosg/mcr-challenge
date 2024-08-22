using System.Text.Json;
using Application.DTO.Application.Motorcycle;
using Application.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class MotorcycleAdapterService : IMotorcycleAdapterService
    {
        private readonly IKafkaConnector kafkaConnector;
        private IProducer<string, string> producer;

        public IConsumer<string, string> Consumer { get; set; }
        ILogger<MotorcycleAdapterService> logger;

        public MotorcycleAdapterService(IKafkaConnector kafkaConnector,
            ILogger<MotorcycleAdapterService> logger)
        {
            this.kafkaConnector = kafkaConnector;
            this.logger = logger;
            this.producer = new ProducerBuilder<string, string>(this.kafkaConnector.ClientConfig).Build();

            Consumer = new ConsumerBuilder<string, string>(this.kafkaConnector.ConsumerConfig).Build();
            Consumer.Subscribe(this.kafkaConnector.Topic);
        }

        public string GetMessageAsync()
        {
            var consumeResult = Consumer.Consume(1000);

            if (consumeResult is null)
                return null;

            return consumeResult.Message.Value;
        }

        public async Task<PersistenceStatus> SendAsync(MotorcycleAddRequestDTO request)
        {
            var key = request.Plate;
            var val = JsonSerializer.Serialize(request);

            var result = await producer.ProduceAsync(this.kafkaConnector.Topic,
                new Message<string, string>
                {
                    Key = key,
                    Value = val
                });

            this.logger.LogInformation($"Customer {request.Plate} {result.Status}");
            return result.Status;
        }
    }
}
