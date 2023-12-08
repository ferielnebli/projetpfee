using ProtoBuf;

namespace WatchDog
{
    [ProtoContract]
    public class HeartBeatMessage
    {

        [ProtoMember(1)]
        public HeartBeatMessageType TypeOfMessage { get; set; }

        [ProtoMember(2)]
        public MicroserviceInfo MicroserviceInfo { get; set; }

        [ProtoMember(3)]
        public TimeSpan WaitTime { get; set; }

        [ProtoMember(4)]
        public string? Token { get; set; }

        [ProtoMember(5)]
        public DateTime DateMessage { get; set; }


        public HeartBeatMessage()
        {
            MicroserviceInfo = new MicroserviceInfo();
        }
        public HeartBeatMessage(MicroserviceInfo microserviceInfo, HeartBeatMessageType typeOfMessage, TimeSpan time, DateTime dateOfMessage, string token)
        {

            this.TypeOfMessage = typeOfMessage;
            this.MicroserviceInfo = microserviceInfo;
            this.WaitTime = time;
            this.DateMessage = dateOfMessage;
            this.Token = token;
        }
    }
}
