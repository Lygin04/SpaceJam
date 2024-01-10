using Core.Event.Model;

namespace UI.Event
{
    public class UiPauseGameEvent : IEventModel
    {
        public const string PAUSEGAME_EVENT = "pauseGame";
        public string EventName => PAUSEGAME_EVENT;
    }
}