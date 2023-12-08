
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Kafka;
using WatchDog;

namespace MicroserviceBase
{
    public class MicroserviceBaseWorker : BackgroundService
    {
        private readonly ILogger<MicroserviceBaseWorker> _logger;
        private readonly IProducer<HeartBeatMessage> _producer;
        private readonly IConsumer<HeartBeatMessage> _consumer;

        private CancellationToken _token;

        private string _tokenGuid = string.Empty;
        public MicroserviceBaseWorker(ILogger<MicroserviceBaseWorker> logger, IProducer<HeartBeatMessage> producer, IConsumer<HeartBeatMessage> consumer)
        {
            _logger = logger;

            _producer = producer;
            _consumer = consumer;
            _consumer._receiveEvent += ManageReceivedMessageFromWatchDog;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                MicroserviceInfo authentificationInfo = new MicroserviceInfo(MicroserviceConfig.Name, MicroserviceConfig.Type, LastUpDate.FirstRun);
                TimeSpan timeOfWaitMessage = TimeSpan.FromSeconds(20);
                _tokenGuid = MicroserviceConfig.GenerateToken();

                HeartBeatMessage message = new HeartBeatMessage(authentificationInfo, HeartBeatMessageType.HiWatchDog, timeOfWaitMessage, DateTime.Now, _tokenGuid);

                _producer.sendMessage(message, cancellationToken);

                _logger.LogInformation(message.ToString());
            });
            return Task.CompletedTask;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                CancellationToken _token = new CancellationToken();
            }
        }


        private void ManageReceivedMessageFromWatchDog(HeartBeatMessage heartBeat)
        {
            _logger.LogInformation(heartBeat.ToString());

            if (heartBeat.Token == _tokenGuid)
            {
                while (!_token.IsCancellationRequested)
                {
                    heartBeat.MicroserviceInfo.LastUpDate = LastUpDate.Running;
                    HeartBeatMessage message = new HeartBeatMessage(heartBeat.MicroserviceInfo, HeartBeatMessageType.IamAlive, heartBeat.WaitTime, DateTime.Now, _tokenGuid);

                    _producer.sendMessage(message, _token);

                    _logger.LogInformation(message.ToString());

                    Thread.Sleep(heartBeat.WaitTime);
                }
            }
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service Authentification stopping !");
            return Task.CompletedTask;
        }


    }
}



