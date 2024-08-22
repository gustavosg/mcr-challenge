using Confluent.Kafka;

namespace Application.Interfaces
{
    public interface IKafkaConnector
    {
        ConsumerConfig ConsumerConfig { get; }
        ClientConfig ClientConfig { get; }
        IDictionary<string, string> KafkaConfig { get; }
        string Server { get; }
        string Topic { get; }
    }
}
