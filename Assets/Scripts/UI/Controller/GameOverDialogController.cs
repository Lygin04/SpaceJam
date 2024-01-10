using Core;
using Game.Spawn.Event;
using TMPro;
using UI.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Controller
{
    public class GameOverDialogController : MonoBehaviour, IDialogController
    {
        [SerializeField] private Button _goToMenu;
        [SerializeField] private Button _restartGame;
        [SerializeField] private TextMeshProUGUI _score;
        private GameContext _gameContext;

        [Header("Sound")] [SerializeField] private AudioSource _buttonClickSound;

        private void Awake()
        {
            _gameContext = GameContext.Instance;
            _restartGame.onClick.AddListener(OnRestartButtonClick);
            _goToMenu.onClick.AddListener(OnMenuButtonClick);
        }

        private void OnMenuButtonClick()
        {
            GameContext.ClearContext();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if (_buttonClickSound == null || _buttonClickSound.clip == null)
            {
                return;
            }
            _buttonClickSound.Play();
        }

        private void OnRestartButtonClick()
        {
            _gameContext.EventDispatcher.Dispatch(new RequestWaveEvent(0));
            _gameContext.EventDispatcher.Dispatch(new UiGameplayEvent());
            _gameContext.PlayerTimerService.StartTimer();
            _gameContext.PlayerScoreService.ResetScore();
            if (_buttonClickSound == null || _buttonClickSound.clip == null)
            {
                return;
            }
            _buttonClickSound.Play();
        }

        public void Show()
        {
            _score.text = "Score: " + _gameContext.PlayerScoreService.Score;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}