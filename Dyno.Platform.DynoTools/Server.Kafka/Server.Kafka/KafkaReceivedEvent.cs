namespace Server.Kafka
{
    public delegate void KafkaReceivedEvent<T>(T message) where T : class, new();
}
