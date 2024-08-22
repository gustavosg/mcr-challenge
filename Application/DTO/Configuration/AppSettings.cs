namespace Application.DTO.Configuration
{
    public class AppSettings
    {
        public DatabaseSettings Database { get; set; }
        public KafkaSettings Kafka { get; set; }
        public JwtOptionsSettings JwtOptions { get; set; }
    }
}
