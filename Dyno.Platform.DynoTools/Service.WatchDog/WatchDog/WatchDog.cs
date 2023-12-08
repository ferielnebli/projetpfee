namespace WatchDog
{
    public sealed class WatchDog
    {
        private static readonly object lockInstance = new object();

        private static WatchDog? _instance = null;

        Dictionary<MicroserviceInfo, MicroserviceWatcher> _microservicesDic = new Dictionary<MicroserviceInfo, MicroserviceWatcher>();

        private ILogger Logger { get => StaticLoggerFactory.GetStaticLogger<WatchDog>(); }

        private WatchDog() { }

        public static WatchDog Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockInstance)
                    {
                        if (_instance == null) { _instance = new WatchDog(); }
                    }
                }
                return _instance;
            }
        }

        public HeartBeatMessage? ManageHeartBeat(HeartBeatMessage heartBeat)
        {
            switch (heartBeat.TypeOfMessage)
            {
                case HeartBeatMessageType.HiWatchDog:
                    if (heartBeat.MicroserviceInfo.Guid == Guid.Empty && !_microservicesDic.ContainsKey(heartBeat.MicroserviceInfo))
                    {
                        MicroserviceWatcher NewWatcher = new MicroserviceWatcher(heartBeat);
                        _microservicesDic.Add(heartBeat.MicroserviceInfo, NewWatcher);
                        HeartBeatMessage heartBeatMessageHi = new HeartBeatMessage(heartBeat.MicroserviceInfo, HeartBeatMessageType.HiMicroservice, heartBeat.WaitTime, DateTime.Now, heartBeat.Token);

                        return heartBeatMessageHi;
                    }
                    else
                    {
                        HeartBeatMessage heartBeatMessageExist = new HeartBeatMessage(heartBeat.MicroserviceInfo, HeartBeatMessageType.MicroserviceExist, heartBeat.WaitTime, DateTime.Now, heartBeat.Token);
                        return heartBeatMessageExist;
                    }
                case HeartBeatMessageType.IamAlive:
                    try
                    {
                        if (!_microservicesDic.ContainsKey(heartBeat.MicroserviceInfo))
                        {
                            HeartBeatMessage heartBeatMessageInexist = new HeartBeatMessage(heartBeat.MicroserviceInfo, HeartBeatMessageType.MicroserviceInexist, heartBeat.WaitTime, DateTime.Now, heartBeat.Token);
                            return heartBeatMessageInexist;
                        }
                        MicroserviceWatcher watcher = _microservicesDic[heartBeat.MicroserviceInfo];
                        watcher.ManageMessage(heartBeat);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    break;
                case HeartBeatMessageType.ByeWatchDog:
                    //destructor
                    _microservicesDic.Remove(heartBeat.MicroserviceInfo);
                    HeartBeatMessage heartBeatMessageBye = new HeartBeatMessage(heartBeat.MicroserviceInfo, HeartBeatMessageType.MicroserviceBye, heartBeat.WaitTime, DateTime.Now, heartBeat.Token);
                    return heartBeatMessageBye;
            }
            HeartBeatMessage heartBeatMessageError = new HeartBeatMessage(heartBeat.MicroserviceInfo, HeartBeatMessageType.Error, heartBeat.WaitTime, DateTime.Now, heartBeat.Token);
            return heartBeatMessageError;
        }
    }
}
