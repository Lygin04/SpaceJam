using System;
using Core;
using Game.Spawn.Event;
using UI.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Controller
{
    public class PauseDialogController: MonoBehaviour, IDialogController
    {
       [SerializeField] private Button _continueButton;
       [SerializeField] private Button _exitGameButton;
       [SerializeField] private Button _goToMenu;
       private GameContext _gameContext;

       [Header("Sound")] [SerializeField] private AudioSource _buttonClickSound;
       private void Awake()
       {
           _gameContext = GameContext.Instance;
           _continueButton.onClick.AddListener(OnContinueButtonClick);
           _exitGameButton.onClick.AddListener(OnExitGameButtonClick);
           _goToMenu.onClick.AddListener(OnGoToMenuButtonClick);
       }

       private void OnGoToMenuButtonClick()
       {
           GameContext.ClearContext();
           SceneManager.LoadScene(SceneManager.GetActiveScene().name);
           if (_buttonClickSound == null || _buttonClickSound.clip == null)
           {
               return;
           }
           _buttonClickSound.Play();
       }

       private void OnExitGameButtonClick()
       {
           Application.Quit();
       }

       private void OnContinueButtonClick()
       {
           _gameContext.EventDispatcher.Dispatch(new UiGameplayEvent());
           if (_buttonClickSound == null || _buttonClickSound.clip == null)
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