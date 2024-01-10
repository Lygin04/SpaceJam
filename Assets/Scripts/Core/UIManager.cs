using System.Collections.Generic;
using Event.Service;
using UI.Data;
using UI.Event;
using UnityEngine;

namespace Core
{
    public class UIManager : MonoBehaviour
    {
        private EventDispatcher _eventDispatcher;
        private GameObject _rootUi;

        private Dictionary<UIDialogType, IDialogController> _dialogControllers =
            new Dictionary<UIDialogType, IDialogController>();

        private void OnGamePaused(UiPauseGameEvent obj)
        {
            HideAll();
            _dialogControllers[UIDialogType.PauseDialog].Show();
        }


        private void OnStartGame(UiStartGameEvent uiStart)
        {
            HideAll();
            _dialogControllers[UIDialogType.MainDialog].Show();
        }

        private void OnGameplayStarted(UiGameplayEvent obj)
        {
            HideAll();
            _dialogControllers[UIDialogType.GameplayDialog].Show();
        }


        private void OnDestroy()
        {
            _eventDispatcher.RemoveListener<UiStartGameEvent>(UiStartGameEvent.START_GAME_EVENT, OnStartGame);
            _eventDispatcher.RemoveListener<UiGameplayEvent>(UiGameplayEvent.GAMEPLAY_EVENT, OnGameplayStarted);
            _eventDispatcher.RemoveListener<UiPauseGameEvent>(UiPauseGameEvent.PAUSEGAME_EVENT, OnGamePaused);
            _eventDispatcher.RemoveListener<UiGameOverEvent>(UiGameOverEvent.GAMEOVER_EVENT, OnGameOver);
        }


        public void Init(GameContext gameContext, GameObject root)
        {
            _eventDispatcher = gameContext.EventDispatcher;
            _eventDispatcher.AddListener<UiStartGameEvent>(UiStartGameEvent.START_GAME_EVENT, OnStartGame);
            _eventDispatcher.AddListener<UiGameplayEvent>(UiGameplayEvent.GAMEPLAY_EVENT, OnGameplayStarted);
            _eventDispatcher.AddListener<UiPauseGameEvent>(UiPauseGameEvent.PAUSEGAME_EVENT, OnGamePaused);
            _eventDispatcher.AddListener<UiGameOverEvent>(UiGameOverEvent.GAMEOVER_EVENT, OnGameOver);
            _rootUi = root;
            CreateAllDialogs(gameContext);
        }

        private void OnGameOver(UiGameOverEvent obj)
        {
            _dialogControllers[UIDialogType.GameplayDialog].Hide();
            _dialogControllers[UIDialogType.GameOverDialog].Show();
        }

        private void CreateAllDialogs(GameContext gameContext)
        {
            GameObject prefab = gameContext.ResourceService.Load<GameObject>("Prefab/UI/MainDialog");
            GameObject dialog = Instantiate(prefab, _rootUi.transform);
            IDialogController dialogController = dialog.GetComponent<IDialogController>();
            _dialogControllers.Add(UIDialogType.MainDialog, dialogController);

            GameObject prefabGameplay = gameContext.ResourceService.Load<GameObject>("Prefab/UI/GameplayMenu");
            GameObject dialogGameplay = Instantiate(prefabGameplay, _rootUi.transform);
            IDialogController dialogGameplayController = dialogGameplay.GetComponent<IDialogController>();
            _dialogControllers.Add(UIDialogType.GameplayDialog, dialogGameplayController);

            GameObject prefabGameOverDialog = gameContext.ResourceService.Load<GameObject>("Prefab/UI/GameOverDialog");
            GameObject dialogGameOver = Instantiate(prefabGameOverDialog, _rootUi.transform);
            IDialogController dialogGameOverController = dialogGameOver.GetComponent<IDialogController>();
            _dialogControllers.Add(UIDialogType.GameOverDialog, dialogGameOverController);

            GameObject prefabPauseDialog = gameContext.ResourceService.Load<GameObject>("Prefab/UI/PauseDialog");
            GameObject dialogPauseGame = Instantiate(prefabPauseDialog, _rootUi.transform);
            IDialogController dialogPauseGameController = dialogPauseGame.GetComponent<IDialogController>();
            _dialogControllers.Add(UIDialogType.PauseDialog, dialogPauseGameController);
            HideAll();
        }

        private void HideAll()
        {
            foreach (var keyValuePair in _dialogControllers)
            {
                keyValuePair.Value.Hide();
            }
        }
    }
}