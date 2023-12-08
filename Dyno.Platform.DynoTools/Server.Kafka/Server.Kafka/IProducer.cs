namespace Server.Kafka
{
    public interface IProducer<T> where T : class, new()
    {
        string Topic { get; }
        void sendMessage(T message, CancellationToken cancellationToken);
    }
}
