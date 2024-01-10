using System;
using Core;
using DG.Tweening;
using Event.Service;
using Game.Model;
using Game.Player.Event;
using Game.Spawn.Event;
using Game.UFO.Event;
using Game.UFO.Model;
using UI.Event;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Game.UFO.Controller
{
    public class UFOController : MonoBehaviour, ITargetModel
    {
        [SerializeField] private GameObject _vfxObjectDestroy;
        [SerializeField] private GameObject _vfxObjectTarget;

        [Header("Sound")] 
        [SerializeField] private AudioSource _deadSound;
        [SerializeField] private AudioSource _endGameSound;
        
        private EventDispatcher _eventDispatcher;

        private int _scoreForFullCoows;
        private int _countTargetsFullUFO;


        private int _countTargetsTaked;
        private int _health;

        
        private float _speedDisk;
        private float _maxRightPos, _maxLeftPos;
        private bool _isAxisX;
        private bool _isMovementRight = true;
        private float _movementAxisTime = 0f;
        private Tween _flyInDirection;


        private bool _isFlyStart = false;
        private void Awake()
        {
            _eventDispatcher = GameContext.Instance.EventDispatcher;
            _eventDispatcher.AddListener<UiGameOverEvent>(UiGameOverEvent.GAMEOVER_EVENT, OnGameOver);
        }

        public void SetUpParams(int scoreForFull, int countTargetToTake, int health, float maxRightPos, float maxLeftPos,
            bool isAxisX, float speed)
        {
            _scoreForFullCoows = scoreForFull;
            _countTargetsFullUFO = countTargetToTake;

            _maxRightPos = maxRightPos;
            _maxLeftPos = maxLeftPos;
            _isAxisX = isAxisX;
            _speedDisk = speed;
            
            _countTargetsTaked = 0;
            _health = health;
        }


        private void DelayBeforeDestroy()
        {
            Observable.Timer(TimeSpan.FromSeconds(1.5))
                .Subscribe(_ =>
                {
                    _flyInDirection.Kill();
                    _eventDispatcher.Dispatch(new RequestWaveEvent()); //После отправки этого текущая тарелка удалится
                });
        }

        private void FixedUpdate()
        {
            DiskMovement();
        }

        private void DiskMovement()
        {
            if (_isFlyStart)
            {
                Vector3 endFlyPoint = new Vector3(Random.Range(-100f, 100f),
                    Random.Range(transform.position.y + 50f, transform.position.y + 100f),
                    Random.Range(100f, -100f));
                transform.position =
                    Vector3.LerpUnclamped(transform.position, endFlyPoint, _speedDisk * 0.2f * Time.deltaTime);
            }

            if (_speedDisk != -1f)
            {
                _movementAxisTime += _speedDisk * Time.deltaTime;
                if (_movementAxisTime > 1f)
                {
                    _movementAxisTime = 0f;
                    if (_isMovementRight)
                        _isMovementRight = false;
                    else
                        _isMovementRight = true;
                }

                Vector3 destinationPoint = Vector3.zero;
                if (_isAxisX)
                {
                    if (_isMovementRight)
                    {
                        destinationPoint = new Vector3(_maxRightPos, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        destinationPoint = new Vector3(_maxLeftPos, transform.position.y, transform.position.z);
                    }
                }
                else
                {
                    if (_isMovementRight)
                    {
                        destinationPoint = new Vector3(transform.position.x, transform.position.y, _maxRightPos);
                    }
                    else
                    {
                        destinationPoint = new Vector3(transform.position.x, transform.position.y, _maxLeftPos);
                    }
                }

                transform.position =
                    Vector3.LerpUnclamped(transform.position, destinationPoint, _speedDisk * Time.deltaTime);
            }
        }
        
        private void ContactWithBomb(BulletShootModel bullet)
        {
            _health--;
            if (_health > 0)
            {
                return;
            }

            var bulletStartShootPosition = bullet.StartShootPosition;
            bulletStartShootPosition.y = 0;
            FlyToPLayerPosition(-bulletStartShootPosition);
            DelayBeforeDestroy();
            _vfxObjectDestroy.SetActive(true);
        }



        private void ContactWithCow(BulletShootModel bullet)
        {
            _countTargetsTaked++;
            if (_countTargetsTaked < _countTargetsFullUFO)
            {
                return;
            }
            FlyToPLayerPosition(bullet.StartShootPosition);
            DelayBeforeDestroy();
            _eventDispatcher.Dispatch(new ScoreEvent(_scoreForFullCoows));
            _vfxObjectTarget.SetActive(true);
        }

        private void FlyToPLayerPosition(Vector3 bulletStartShootPosition)
        {
            bulletStartShootPosition.y += 150;
            bulletStartShootPosition.x += 20;
            var direction = (bulletStartShootPosition - transform.position).normalized;
            _flyInDirection = transform.DOMove(direction * 10, 1.5f)
               
                .SetLink(gameObject);
        }

        private void OnGameOver(UiGameOverEvent obj)
        {
            _speedDisk = 0f;
            if (_endGameSound == null || _endGameSound.clip == null)
            {
                return;
            }
            _endGameSound.Play();
        }

        private void OnDestroy()
        {
            _flyInDirection?.Kill();
        }

        public void Contact(BulletShootModel bullet, LevelInfo.BulletData bulletData)
        {
            if (bulletData.BulletType == Bullet.cow)
            {
                ContactWithCow(bullet);
            }
            else
            {
                ContactWithBomb(bullet);
            }

            //TODO: Some Behaviour with VFX
            _eventDispatcher.Dispatch(new UFOContactEvent(bulletData.BulletType));
            if (_deadSound == null || _deadSound.clip == null)
            {
                return;
            }
            _deadSound.Play();
        }
    }
}