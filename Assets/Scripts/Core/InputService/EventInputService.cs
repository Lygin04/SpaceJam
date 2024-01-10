using Core.InputService.Event;
using Event.Service;
using UnityEngine;

namespace Core.InputService
{
    public class EventInputService : MonoBehaviour
    {
        private EventDispatcher _eventDispatcher;
        private bool touchStarted;

        public void Init(EventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
        }

        public void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (Input.GetMouseButtonDown(0))
            {
                _eventDispatcher.Dispatch(new PlayerTouchedEvent(Input.mousePosition));
            }
#else

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    return;
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    touchStarted = false;
                }

                if (touch.phase == TouchPhase.Began && touchStarted)
                {
                    return;
                }

                if (touch.phase == TouchPhase.Began)
                {
                    touchStarted = true;

                    _eventDispatcher.Dispatch(new PlayerTouchedEvent(Input.mousePosition));

                }


            }
#endif
        }

    }
}