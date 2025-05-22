namespace GameLogic
{
    public struct EventOnApplicationPause : IEvent
    {
        public bool pause;
        public EventOnApplicationPause(bool inPause)
        {
            pause = inPause;
        }
    }
    public struct UpdateAccountInfo : IEvent { }
}