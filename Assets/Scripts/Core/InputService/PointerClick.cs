using System;
using Core.InputService.Event;
using Event.Service;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.InputService
{
    public class PointerClick : MonoBehaviour, IPointerUpHandler , IPointerDownHandler
    {
        private EventDispatcher _eventDispatcher;
        private void Awake()
        {
            _eventDispatcher = GameContext.Instance.EventDispatcher;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var component = eventData.pointerPress.gameObject.GetComponentInChildren<Button>();
            if (component == null)
            {
                _eventDispatcher.Dispatch(new PlayerTouchedEvent(Input.mousePosition));
            }
        }
    }
}