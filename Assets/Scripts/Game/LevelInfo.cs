using System;
using System.Collections.Generic;
using System.Linq;
using Game.UFO.Controller;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "LevelInfo", menuName = "Level/LevelInfo")]
    public class LevelInfo : ScriptableObject
    {
        [Serializable]
        public class BulletData
        {
            public GameObject Prefab;
            public Bullet BulletType;
            public int Damage = 1;
        }
        
        [Serializable]
        public class DiscWaveSpawn
        {
            public UFOController Prefab;
            public float TimeSelectDirectionInSeconds = 1;
            public float TimeSelectPowerInSeconds = 1;
            public float speed = 0.1f; 
            public bool isAxisX = true;
            public float MaxLeftPosition = 10;
            public float MaxRightPosition = -10;
            public float MaxLeftAngle = -45;
            public float MaxRightAngle = 45;
            public int ScoreForFull = 2;
            public int CountTargetsToTake = 1;
            public int Health = 1;
            public Vector3 UFOPosition;
        }
        
        
        [SerializeField] private string _id;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Bullet[] _queue;
        [SerializeField] private List<BulletData> _bullets = new List<BulletData>();
        [SerializeField] private List<DiscWaveSpawn> _spawn = new List<DiscWaveSpawn>();
        [SerializeField] private float _levelTime;

        public float LevelTime => _levelTime;

        public string id => _id;
        public GameObject prefab => _prefab;
        public Bullet[] Queue => _queue;

        public DiscWaveSpawn GetSpawnInfo(int index)
        {
            return _spawn[index];
        }
        public BulletData GetBulletDataByType(Bullet bullet)
        {
            return _bullets.First(x => x.BulletType == bullet);
        }
        public int countBullet
        {
            get
            {
                return Queue.Length;
            }
        }
    }
    
    public enum Bullet
    {
        cow,
        bomb
    }
}