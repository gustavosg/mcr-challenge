namespace Application.DTO.Configuration
{
    public class KafkaSettings
    {
        public string Server { get; set; }
        public string Topic { get; set; }
        public string DefaultLimit { get; set; }
    }
}
