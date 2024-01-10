using Core.Event.Model;

namespace Game.Player.Event
{
    public class ReloadEvent : IEventModel
    {
        public const string RELOAD_PLAYER_EVENT = "reloadEvent";
        public string EventName => RELOAD_PLAYER_EVENT;
    }
}