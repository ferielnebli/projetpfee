using Confluent.Kafka;


namespace Server.Kafka
{
    public class Producer<T> : IProducer<T> where T : class, new()
    {
        private IProducer<Null, byte[]> _kafkaProducer;
        private readonly KafkaConfig _boostrapServerKafka;

        public string Topic { get; }

        public Producer(string topic, KafkaConfig kafkaConfig)
        {
            Topic = topic;
            _boostrapServerKafka = kafkaConfig;

            var config = new AdminClientConfig
            {
                BootstrapServers = $"{_boostrapServerKafka.IP}:{_boostrapServerKafka.Port}"
            };


            _kafkaProducer = new ProducerBuilder<Null, byte[]>(config).Build();
        }

        public async void sendMessage(T message, CancellationToken cancellationToken)
        {
           await _kafkaProducer.ProduceAsync(Topic, new Message<Null, byte[]>()
            {
                Value = KafkaMessage<T>.TCSerialize(message)
            }, cancellationToken);

            _kafkaProducer.Flush(TimeSpan.FromSeconds(10));
        }

        public void stop()
        {
            _kafkaProducer?.Dispose();
        }
    }
}
