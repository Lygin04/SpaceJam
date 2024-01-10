using Core.Event.Model;

namespace UI.Event
{
    public class UiGameOverEvent : IEventModel
    {
        public const string GAMEOVER_EVENT = "gameOver";
        public string EventName => GAMEOVER_EVENT;
    }
}