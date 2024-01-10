using Core.Event.Model;

namespace Game.Player.Event
{
    public class ScoreEvent : IEventModel
    {
        public const string SCORE_EVENT_NAME = "ScoreEvent";
        public string EventName => SCORE_EVENT_NAME;
        private readonly int _countScore;

        public ScoreEvent(int countScore)
        {
            _countScore = countScore;
        }

        public int CountScore => _countScore;
    }
}