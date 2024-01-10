using System;
using System.Collections.Generic;
using Core.InputService;
using Core.ResourceService;
using DG.Tweening;
using Event.Service;
using Game;
using Game.Level;
using Game.Player.Repository;
using Game.Player.Service;
using Game.Service;
using Game.Spawn;
using UnityEngine;

namespace Core
{
    public class GameContext
    {
        private const string PATH_TO_INPUT = "Prefab/EventSystem";
        private List<IDisposable> _disposablesService;
        private static GameContext _gameContext;
        private bool _inited;

        public static GameContext Instance
        {
            get
            {
                if (_gameContext == null)
                {
                    _gameContext = new GameContext();
                }

                return _gameContext;
            }
        }

        public EventDispatcher EventDispatcher { get; set; }


        public CacheResourceService ResourceService { get; set; }

        public PlayerService PlayerService { get; set; }

        public EventInputService EventInputService { get; set; }
        public LevelInfo LevelInfo { get; set; }
        
        public SpawnService SpawnService { get; set; }
        public PlayerTimerService PlayerTimerService { get; set; }
        
        public PlayerScoreService PlayerScoreService { get; set; }

        public BulletService BulletService { get; set; }
        public LevelService LevelService { get; set; }

        private GameContext()
        {
        }

        public void Init()
        {
            if (_inited)
            {
                return;
            }

            DOTween.Init();
            ResourceService = new CacheResourceService();
            EventDispatcher = new EventDispatcher();
            EventInputService = ResourceService.Load<EventInputService>(PATH_TO_INPUT);
            EventInputService = GameObject.Instantiate<EventInputService>(EventInputService);
            EventInputService.Init(EventDispatcher);
            PlayerService = new PlayerService(new PlayerRepository(), ResourceService);
            LevelInfo = ResourceService.Load<LevelInfo>("LevelInfo");
            PlayerScoreService = new PlayerScoreService(EventDispatcher);
            PlayerTimerService = new PlayerTimerService(EventDispatcher, LevelInfo);
            BulletService = new BulletService(LevelInfo, EventDispatcher);
            SpawnService = new SpawnService(LevelInfo, EventDispatcher, PlayerService);
            LevelService = new LevelService(LevelInfo);
            _inited = true;

            _disposablesService = new List<IDisposable>()
            {
                SpawnService,
                PlayerTimerService
            };
        }


        private void Clear()
        {
            foreach (IDisposable disposable in _disposablesService)
            {
                disposable.Dispose();
            }

            _disposablesService.Clear();
        }


        public static void ClearContext()
        {
            _gameContext.Clear();
            _gameContext = null;
        }
    }
}