using Game.Spawn.Event;
using UI.Event;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        //тут ссылки на все
        [SerializeField] private GameObject _uiContainer;

        [SerializeField] private GameObject _gameRoot;
        [SerializeField] private UIManager _uiManager;

        private GameContext _gameContext;

        private void Awake()
        {
            _gameContext = GameContext.Instance;
            _gameContext.Init();
            _uiManager.Init(_gameContext, _uiContainer);
            _gameContext.SpawnService.Init(_gameRoot);
            _gameContext.PlayerService.Init(_gameRoot);
            _gameContext.LevelService.Init(_gameRoot);
            _gameContext.EventDispatcher.Dispatch(new UiStartGameEvent());
            _gameContext.LevelService.CreateLevel();
        }
    }
}