using Core.Event.Model;
using UnityEngine;

namespace Core.InputService.Event
{
    public class PlayerTouchedEvent : IEventModel
    {
        public const string TOUCH_EVENT = "touchEvent";

        public PlayerTouchedEvent(Vector2 touchPosition)
        {
            _touchPosition = touchPosition;
        }

        public string EventName => TOUCH_EVENT;

        private readonly Vector2 _touchPosition;

        public Vector2 TouchPosition => _touchPosition
        ;
    }
}