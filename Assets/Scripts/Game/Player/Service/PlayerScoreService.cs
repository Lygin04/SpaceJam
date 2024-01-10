using System;
using Event.Service;
using Game.Player.Event;

namespace Game.Player.Service
{
    public class PlayerScoreService : IDisposable
    {
        private readonly EventDispatcher _eventDispatcher;
        private int _score;

        public PlayerScoreService(EventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
            _eventDispatcher.AddListener<ScoreEvent>(ScoreEvent.SCORE_EVENT_NAME, OnScoreChanged);
        }

        private void OnScoreChanged(ScoreEvent score)
        {
            _score += score.CountScore;
        }

        public int Score => _score;

        public void Dispose()
        {
            _score = 0;
            _eventDispatcher.RemoveListener<ScoreEvent>(ScoreEvent.SCORE_EVENT_NAME, OnScoreChanged);
        }

        public void ResetScore()
        {
            _score = 0;
        }
    }
}