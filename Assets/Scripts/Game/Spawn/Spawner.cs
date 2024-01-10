using System;
using UnityEngine;

namespace Game
{
    public class Spawner : MonoBehaviour
    {
        public static Spawner Instance;
        [Header("Bullet")]
        [SerializeField] private GameObject cow;
        [SerializeField] private GameObject bomb;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SpawnBulletsFromQueue(LevelInfo _levelInfo)
        {
            foreach (Bullet bulletType in _levelInfo.Queue)
            {
                if (bulletType == Bullet.cow)
                    Instantiate(cow);
                else if (bulletType == Bullet.bomb)
                    Instantiate(bomb);
            }
        }

    }
}