using Confluent.Kafka;
using System.Threading;

namespace WebAPI.Services
{
    public class KafkaConsumerService
    {
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly ConsumerConfig _consumerConfig;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public KafkaConsumerService(ILogger<KafkaConsumerService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration.GetValue<string>("BootstrapServer"),
                GroupId = "1",// Identificador do grupo de consumidores
                AutoOffsetReset = AutoOffsetReset.Earliest // Caso não haja offset (registro de onde começar), começa do mais antigo
            };
            _cancellationTokenSource = new CancellationTokenSource();// Inicializa o TokenSource para poder cancelar o consumo posteriormente
        }

        public async Task StartConsuming(string topic)
        {
            var cancellationToken = _cancellationTokenSource.Token;
            // Inicia o consumo assíncrono
            await ConsumeMessagesAsync( cancellationToken, topic);
        }


        public async Task ConsumeMessagesAsync(CancellationToken cancellation,string topic)
        {
            // Cria o consumidor Kafka com as configurações fornecidas
            using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();

            try
            {
                consumer.Subscribe(topic);
                _logger.LogInformation($"Iniciando consumo do tópico: {topic}");
                
                // Fica consumindo as mensagens até que o cancelamento seja solicitado
                while (!cancellation.IsCancellationRequested)
                {
                    try
                    {
                        // Consume a próxima mensagem no tópico. O método `Consume` pode bloquear até que uma nova mensagem chegue.
                        var consumeResult = consumer.Consume(cancellation);
                        _logger.LogInformation($"Mensagem consumida: {consumeResult.Message.Value}");

                    }
                    catch(ConsumeException consumeEx)
                    {
                        _logger.LogError($"Erro ao consumir mensagem do tópico {topic}. Detalhes: {consumeEx.Message}");

                    }
                }
            }
            catch(ConsumeException ex)
            {
                // Se ocorrer um erro geral na tentativa de consumir do tópico, ele é registrado aqui
                _logger.LogError($"Erro geral ao consumir do tópico {topic}. Detalhes: {ex.Message}");
            }
            finally
            {
                // Garante que o consumidor será fechado após a execução
                consumer.Close();

                // Loga que o consumidor foi fechado
                _logger.LogInformation("Fechando consumidor e interrompendo o serviço.");
            }
        }
        public void StopConsuming()
        {
            // Solicita o cancelamento do consumo
            _cancellationTokenSource.Cancel();
        }

    }
}
