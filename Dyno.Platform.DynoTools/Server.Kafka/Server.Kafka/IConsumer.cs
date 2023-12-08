namespace Server.Kafka
{
    public interface IConsumer<T> where T : class, new()
    {
        event KafkaReceivedEvent<T> _receiveEvent;
        string Topic { get; }
    }
}
