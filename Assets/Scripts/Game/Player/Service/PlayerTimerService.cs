using System;
using Event.Service;
using Game.Player.Event;
using UI.Event;
using UniRx;

namespace Game.Player.Service
{
    public class PlayerTimerService : IDisposable
    {
        private readonly EventDispatcher _eventDispatcher;
        private readonly LevelInfo _levelInfo;
        private CompositeDisposable _timerDisposable;
        private DateTime _endLevelTime;

        public PlayerTimerService(EventDispatcher eventDispatcher, LevelInfo levelInfo)
        {
            _eventDispatcher = eventDispatcher;
            _levelInfo = levelInfo;
        }


        public void StartTimer()
        {
            _timerDisposable?.Dispose();
            _timerDisposable = new CompositeDisposable();
            _endLevelTime = DateTime.Now.AddSeconds(_levelInfo.LevelTime);
            Observable.Timer(TimeSpan.FromSeconds(_levelInfo.LevelTime))
                .Subscribe(_ => { OnTimerEnded(); })
                .AddTo(_timerDisposable);
        }

        private void OnTimerEnded()
        {
            _eventDispatcher.Dispatch(new LevelFinishedEvent());
            _eventDispatcher.Dispatch(new UiGameOverEvent());
        }

        public TimeSpan GetTimeBeforeEnd()
        {
            return _endLevelTime - DateTime.Now;
        }

        public void Dispose()
        {
            _timerDisposable?.Dispose();
        }
    }
}