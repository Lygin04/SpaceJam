using Event.Service;

namespace Game.Service
{
    public class BulletService
    {
        private readonly LevelInfo _levelInfo;
        private readonly EventDispatcher _eventDispatcher;
        private int _bulletIndex;

        public BulletService(LevelInfo levelInfo, EventDispatcher eventDispatcher)
        {
            _levelInfo = levelInfo;
            _eventDispatcher = eventDispatcher;
        }

        public LevelInfo.BulletData GetBulletData()
        {
            if (_levelInfo.Queue.Length <= _bulletIndex + 1)
            {
                _bulletIndex = 0;
            }
            else
            {
                _bulletIndex++;
            }

            var bullet = _levelInfo.Queue[_bulletIndex];
            LevelInfo.BulletData bulletDataByType = _levelInfo.GetBulletDataByType(bullet);
            return bulletDataByType;
        }
    }
}