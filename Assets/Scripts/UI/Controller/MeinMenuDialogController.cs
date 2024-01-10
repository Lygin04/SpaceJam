using System;
using Core;
using Game.Spawn.Event;
using UI.Event;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controller
{
    public class MeinMenuDialogController: MonoBehaviour, IDialogController
    {
        [SerializeField] private Button _startButton;
        private GameContext _gameContext;

        [Header("Sound")] [SerializeField] private AudioSource _buttonClickSound;
        
        private void Awake()
        {
            _gameContext = GameContext.Instance;
            _startButton.onClick.AddListener(OnStartButtonClick);
        }

        private void OnStartButtonClick()
        {
            _gameContext.PlayerService.SpawnPlayer();
            _gameContext.EventDispatcher.Dispatch(new RequestWaveEvent());
            _gameContext.EventDispatcher.Dispatch(new UiGameplayEvent());
            _gameContext.PlayerTimerService.StartTimer();
            if (_buttonClickSound == null ||_buttonClickSound.clip == null)
            {
                return;
            }
            _buttonClickSound.Play();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}