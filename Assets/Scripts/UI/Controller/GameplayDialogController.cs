using System;
using Core;
using Game.Player.Event;
using Game.Player.Service;
using TMPro;
using UniRx;
using UI.Event;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controller
{
    public class GameplayDialogController: MonoBehaviour, IDialogController
    {
        [SerializeField] private Button _pauseButton;

        [SerializeField] private TextMeshProUGUI _playerScore;
        
        [SerializeField] private TextMeshProUGUI _playerTime;

        private GameContext _gameContext;
        private PlayerTimerService _playerTimerService;
        private PlayerScoreService _playerScoreService;
        private int _currentScore;
        
        [Header("Sound")] [SerializeField] private AudioSource _buttonClickSound;
        
        private void Awake()
        {
            _gameContext = GameContext.Instance;
            _playerTimerService = _gameContext.PlayerTimerService;
            _playerScoreService = _gameContext.PlayerScoreService;
            _gameContext.EventDispatcher.AddListener<ScoreEvent>(ScoreEvent.SCORE_EVENT_NAME, OnScoreChanged);
            _pauseButton.onClick.AddListener(OnPauseButtonClick);
            Observable.Interval(TimeSpan.FromSeconds(0.5))
                .Where(_ => gameObject.activeSelf)
                .Subscribe(_ => UpdateTimer())
                .AddTo(gameObject);
        }

        private void UpdateTimer()
        {
            _playerTime.text = _playerTimerService.GetTimeBeforeEnd().ToString("m\\:ss");
        }

        private void OnDestroy()
        {
            _gameContext.EventDispatcher.RemoveListener<ScoreEvent>(ScoreEvent.SCORE_EVENT_NAME, OnScoreChanged);
        }

        private void OnScoreChanged(ScoreEvent score)
        {
            _currentScore += score.CountScore;
            _playerScore.text = $"{_currentScore}";
        }
        
        

        private void OnPauseButtonClick()
        {
            _gameContext.EventDispatcher.Dispatch(new UiPauseGameEvent());
            if (_buttonClickSound == null || _buttonClickSound.clip == null)
            {
                return;
            }
            _buttonClickSound.Play();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _currentScore = _playerScoreService.Score;
            _playerScore.text = $"{_currentScore}";
        }

        public void Hide()
        {
           gameObject.SetActive(false);
        }
    }
}