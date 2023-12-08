namespace MicroserviceBase
{
    public class MicroserviceConfig
    {
        public static string Name = "MicroserviceBase";

        public static string Type = typeof(MicroserviceBaseWorker).ToString();

        public static string GenerateToken()
        {
            var random = new Random();
            var buffer = new byte[6];
            random.NextBytes(buffer);
            var macAddress = String.Concat(buffer.Select(x => string.Format("{0}:", x.ToString("X2"))).ToArray());
            Guid guid = Guid.NewGuid();
            return $"{guid}{macAddress.TrimEnd(':')}";
        }
    }
}
