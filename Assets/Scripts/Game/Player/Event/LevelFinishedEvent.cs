using Core.Event.Model;

namespace Game.Player.Event
{
    public class LevelFinishedEvent : IEventModel
    {
        public const string LEVEL_FINISHED_EVENT_NAME = "levelFinished";
        public string EventName => LEVEL_FINISHED_EVENT_NAME;
    }
}