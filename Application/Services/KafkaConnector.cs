using Application.DTO.Configuration;
using Application.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class KafkaConnector : IKafkaConnector
    {
        public ConsumerConfig ConsumerConfig { get; }
        public string Server { get; }
        public string Topic { get; }
        public ClientConfig ClientConfig => new ClientConfig(this.KafkaConfig);
        public IDictionary<string, string> KafkaConfig { get; }
        public KafkaConnector(IOptions<KafkaSettings> kafkaSettings)
        {
            this.Server = kafkaSettings.Value.Server;
            this.Topic = kafkaSettings.Value.Topic;
            this.KafkaConfig = new Dictionary<string, string>
            {
                {
                    "bootstrap.servers", this.Server
                }
            };

            this.ConsumerConfig = new ConsumerConfig
            {
                BootstrapServers = this.Server,
                GroupId = $"{this.Topic}-group-0",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };
        }
    }
}
