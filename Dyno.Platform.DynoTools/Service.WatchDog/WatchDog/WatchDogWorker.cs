using Server.Kafka;

namespace WatchDog
{
    public class WatchDogWorker : BackgroundService
    {
        private readonly ILogger<WatchDogWorker> _logger;

        private readonly IConsumer<HeartBeatMessage> _consumer;
        private readonly IProducer<HeartBeatMessage> _producer;

        private CancellationToken _token;

        public WatchDogWorker(ILogger<WatchDogWorker> logger, ILoggerFactory loggerFactory, IConsumer<HeartBeatMessage> consumer, IProducer<HeartBeatMessage> producer)
        {
            _logger = logger;
            StaticLoggerFactory.Initialize(loggerFactory);

            _consumer = consumer;
            _producer = producer;
            _consumer._receiveEvent += ManageReceivedHeartBeatMessage;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _token = stoppingToken;
            }
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        private void ManageReceivedHeartBeatMessage(HeartBeatMessage heartBeat)
        {
            HeartBeatMessage? heartBeatMessageSend = WatchDog.Instance.ManageHeartBeat(heartBeat);

            if (heartBeatMessageSend != null)
            {
                _producer.sendMessage(heartBeatMessageSend, _token);
            }
        }
    }
}