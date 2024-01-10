using Core.ResourceService;
using UnityEngine;

namespace Game.Level
{
    public class LevelService
    {
        private readonly LevelInfo _levelInfo;

        private GameObject _root;
        private GameObject _level;

        public LevelService(LevelInfo levelInfo)
        {
            _levelInfo = levelInfo;
        }

        public void Init(GameObject root)
        {
            _root = root;
        }

        public void CreateLevel()
        {
            _level = GameObject.Instantiate(_levelInfo.prefab, _root.transform);
        }
    }
}