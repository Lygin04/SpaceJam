using Core.Event.Model;

namespace Game.UFO.Event
{
    public class UFOContactEvent : IEventModel
    {
        public const string UFO_CONTACT = "ufoContact";
        public string EventName => UFO_CONTACT;

        private readonly Bullet _bullet;

        public Bullet Bullet => _bullet;

        public UFOContactEvent(Bullet bullet)
        {
            _bullet = bullet;
        }
    }
}