namespace WatchDog
{
    public enum HeartBeatMessageType
    {
        HiWatchDog = 0,
        IamAlive = 1,
        ByeWatchDog = 2,
        HiMicroservice = 3,
        Error = 4,
        MicroserviceExist = 5,
        MicroserviceBye = 7,
        MicroserviceInexist = 8,
    }
}