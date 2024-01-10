using Game.Model;

namespace Game.UFO.Model
{
    public interface ITargetModel
    {
        void Contact(BulletShootModel bullet, LevelInfo.BulletData bulletData);
    }
}