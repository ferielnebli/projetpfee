
namespace WatchDog
{
    public class MicroserviceWatcher
    {
        public State State { get; set; }

        public Stack<HeartBeatMessage> lastHeartBeatMessage = new Stack<HeartBeatMessage>();

        public volatile int missingMessageNumber = 0;
        public bool messageChecked = true;

        private ILogger Logger { get => StaticLoggerFactory.GetStaticLogger<MicroserviceWatcher>(); }


        public MicroserviceWatcher(HeartBeatMessage heartBeat)
        {

            heartBeat.MicroserviceInfo.GenerateGuid();
            lastHeartBeatMessage.Push(heartBeat);
            messageChecked = false;

            CheckMicroservice();

        }

        public void ManageMessage(HeartBeatMessage heartBeat)
        {
            if (!messageChecked)
            {
                lastHeartBeatMessage.Push(heartBeat);
                missingMessageNumber++;
            }
            else
            {
                lastHeartBeatMessage.Clear();
                lastHeartBeatMessage.Push(heartBeat);
                missingMessageNumber = 1;
                messageChecked = false;
            }
        }

        private void CheckMicroservice()
        {
            Task task = Task.Run(() =>
            {
                while (State != State.Red)
                {
                    if (!messageChecked)
                    {
                        State = State.Green;
                        messageChecked = true;
                        Logger.LogInformation($"watchDog {lastHeartBeatMessage.Peek().MicroserviceInfo.Guid} {lastHeartBeatMessage.Peek().MicroserviceInfo.Name} {lastHeartBeatMessage.Peek().MicroserviceInfo.Type} {this.State} {lastHeartBeatMessage.Peek().MicroserviceInfo.LastUpDate}");
                    }

                    TimeSpan timeInterval = DateTime.Now - lastHeartBeatMessage.Peek().DateMessage;
                    if (timeInterval > lastHeartBeatMessage.Peek().WaitTime + TimeSpan.FromSeconds(2))
                    {
                        ChangeStateToBad();
                        Logger.LogWarning($"watchDog {lastHeartBeatMessage.Peek().MicroserviceInfo.Guid} {lastHeartBeatMessage.Peek().MicroserviceInfo.Name} {lastHeartBeatMessage.Peek().MicroserviceInfo.Type} {this.State} {LastUpDate.Error}");
                    }


                    Thread.Sleep(lastHeartBeatMessage.Peek().WaitTime);
                }
                Logger.LogError($"watchDog {lastHeartBeatMessage.Peek().MicroserviceInfo.Guid} {lastHeartBeatMessage.Peek().MicroserviceInfo.Name} {lastHeartBeatMessage.Peek().MicroserviceInfo.Type} {this.State} {LastUpDate.Stopped}");
            });
        }

        private void ChangeStateToBad()
        {
            if (State == State.Green)
            {
                State = State.Orange;
            }
            else if (State == State.Orange)
            {
                State = State.Red;
            }
        }
    }
}
