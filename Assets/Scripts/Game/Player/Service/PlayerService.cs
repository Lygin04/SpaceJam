using Core.ResourceService;
using Game.Player.Controller;
using Game.Player.Model;
using Game.Player.Repository;
using UnityEngine;

namespace Game.Player.Service
{
    public class PlayerService
    {
        private const string PLAYER_PREFAB = "Prefab/Player";
        private readonly PlayerRepository _playerRepository;

        private readonly CacheResourceService _cacheResourceService;
        private PlayerController _playerController;
        private GameObject _root;
        

        private PlayerModel _playerModel;
        public PlayerService(PlayerRepository playerRepository, CacheResourceService cacheResourceService)
        {
            _playerRepository = playerRepository;
            _playerModel = _playerRepository.Get();
            _cacheResourceService = cacheResourceService;
        }

        public void Init(GameObject gameObject)
        {
            _root = gameObject;
        }
        public void SetParams(float timeSelectDirection, float timeSelectPower, float leftMaxAngle, float rightMaxAngle)
        {
            _playerController.SetParams(timeSelectDirection, timeSelectPower, leftMaxAngle, rightMaxAngle);
        }
        public void SpawnPlayer()
        {
            _playerController = _cacheResourceService.Load<PlayerController>(PLAYER_PREFAB);
            _playerController = GameObject.Instantiate(_playerController, _root.transform);
            _playerController.transform.position = new Vector3(0, 0, 0);
        }
    }
}