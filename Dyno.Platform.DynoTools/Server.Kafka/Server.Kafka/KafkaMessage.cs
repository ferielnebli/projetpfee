using ProtoBuf;
namespace Server.Kafka
{
    public class KafkaMessage<T> where T : class, new()
    {
        public static byte[]? TCSerialize(object obj)
        {
            if (obj == null) { return null; }
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, obj);
                    return stream.ToArray();
                }
            }
            catch
            {
                throw;
            }
        }

        public static T TCDeserialize(byte[] data)
        {
            if (data == null) { return new T(); }
            try
            {
                using (var stream = new MemoryStream(data))
                {
                    return Serializer.Deserialize<T>(stream);
                }
            }
            catch { throw; }
        }
    }
}
