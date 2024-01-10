using Core.Event.Model;

namespace UI.Event
{
    public class UiGameplayEvent : IEventModel
    {
        public const string GAMEPLAY_EVENT = "gameplay";
        public string EventName => GAMEPLAY_EVENT;
    }
}