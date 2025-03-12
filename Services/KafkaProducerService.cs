using Confluent.Kafka;

namespace WebApi.Services
{
    public class KafkaProducerService
    {
        private readonly ILogger<KafkaProducerService> _logger;
        private readonly ProducerConfig _producerConfig;

        public KafkaProducerService(ILogger<KafkaProducerService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration.GetValue<string>("BootstrapServer")
            };
        }
       
   
        public async Task<string> SendMessage(string topic,string message)
        {

            try {
                using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
                var result = await producer.ProduceAsync(topic,new Message<Null, string> { Value = message});
                _logger.LogInformation($"{result.Status} - {message}");
                return $"{result.Status} - {message}";

            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao enviar mensagem para o tópico {topic}: {ex.Message}");
                return "Erro ao enviar mensagem";
            }
        }
    }
}
