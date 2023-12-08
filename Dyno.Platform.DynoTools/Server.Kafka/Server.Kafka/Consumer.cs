using Confluent.Kafka;


namespace Server.Kafka
{
    public class Consumer<T> : IDisposable, IConsumer<T> where T : class, new()
    {
        private bool _disposed = false;

        private IConsumer<Ignore, byte[]> _kafkaConsumer;
        public string Topic { get; }

        public event KafkaReceivedEvent<T>? _receiveEvent;

        private readonly KafkaConfig _boostrapServerKafka;


        public Consumer(string topic, KafkaConfig kafkaConfig)
        {
            Topic = topic;
            _boostrapServerKafka = kafkaConfig;

            var config = new ConsumerConfig
            {
                GroupId = _boostrapServerKafka.GroupeId,
                BootstrapServers = $"{_boostrapServerKafka.IP}:{_boostrapServerKafka.Port}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true
            };

            _kafkaConsumer = new ConsumerBuilder<Ignore, byte[]>(config).Build();
            try
            {
                _kafkaConsumer.Subscribe(Topic);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }


            CancellationTokenSource cancellationToken = new CancellationTokenSource();

            Task.Run(() =>
            {
                ReceiveMessage(cancellationToken.Token);
            });
        }

        private void ReceiveMessage(CancellationToken cancellationToken)
        {
            try
            {

                while (!_disposed)
                {
                    var consumer = _kafkaConsumer.Consume(cancellationToken);
                    byte[] message = consumer.Message.Value;
                    Console.WriteLine(message);

                    if (message.Length != 0)
                    {
                        T received = new T();
                        try
                        {
                            _receiveEvent(KafkaMessage<T>.TCDeserialize(message));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _kafkaConsumer.Close();
            }
        }


        public void Dispose()
        {
            _disposed = true;
            _kafkaConsumer.Dispose();
        }
    }
}
