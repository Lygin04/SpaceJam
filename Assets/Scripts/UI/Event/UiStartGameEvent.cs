using Core.Event.Model;

namespace UI.Event
{
    public class UiStartGameEvent :IEventModel
    {
        public const string START_GAME_EVENT = "startGame";
        public string EventName => START_GAME_EVENT;

    }
}