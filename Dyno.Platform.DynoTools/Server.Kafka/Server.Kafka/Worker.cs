
namespace Server.Kafka
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public readonly KafkaConfig _boostrapServerKafka;


        public Worker(ILogger<Worker> logger, KafkaConfig config)
        {
            _logger = logger;
            _boostrapServerKafka = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Producer stopping !");
            return Task.CompletedTask;
        }
    }
}