using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.InputService.Event;
using DG.Tweening;
using Game.Model;
using Game.Service;
using UI.Event;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private const float POWER_COEF = 17;

        [SerializeField] private GameObject _arrowRoot;

        [SerializeField] private Image _progress;

        [SerializeField] private GameObject _rootContainer;

        [Header("Sound")] [SerializeField] private AudioSource _fireSound;

        private float _leftMaxAngle = -45;
        private float _rightMaxAngle = 45;
        private float _timeSelectDirection = 1;
        private float _timeSelectPower = 1;
        private float _power = 10;
        private float _angleY;
        private Sequence _rotationSequence;
        private Sequence _powerSequence;
        private GameContext _gameContext;
        private PlayerState _playerState;
        private RightBodyBullet _bullet;
        private bool isPaused;
        private BulletService _bulletService;

        public void Start()
        {
            _gameContext = GameContext.Instance;
            _bulletService = _gameContext.BulletService;
            _arrowRoot.SetActive(false);
            _progress.fillAmount = 0;
            _gameContext.EventDispatcher.AddListener<PlayerTouchedEvent>(PlayerTouchedEvent.TOUCH_EVENT, OnTouch);
            _gameContext.EventDispatcher.AddListener<UiPauseGameEvent>(UiPauseGameEvent.PAUSEGAME_EVENT, OnPauseEvent);
            _gameContext.EventDispatcher.AddListener<UiGameplayEvent>(UiGameplayEvent.GAMEPLAY_EVENT, OnGamePlayEvent);
            _gameContext.EventDispatcher.AddListener<UiGameOverEvent>(UiGameOverEvent.GAMEOVER_EVENT, OnGameOver);
        }

        private void OnGameOver(UiGameOverEvent obj)
        {
            isPaused = true;
            ResetAllState();
        }

        private void OnGamePlayEvent(UiGameplayEvent obj)
        {
            isPaused = false;
        }

        private void OnPauseEvent(UiPauseGameEvent obj)
        {
            isPaused = true;
        }

        //TODO ;/ sheet
        private bool IsAnyButtonTouched(Vector2 touchPosition)
        {
            var raycastResults = new List<RaycastResult>();
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = touchPosition
            };
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            var count = raycastResults.Count(c =>
                c.gameObject != null && c.gameObject.GetComponentInChildren<Button>() != null);
            return count > 0;
        }

        private void ResetAllState()
        {
            _powerSequence?.Kill();
            _powerSequence = null;
            _rotationSequence?.Kill();
            _rotationSequence = null;
            _playerState = PlayerState.NotSelected;
            _progress.fillAmount = 0;
            _arrowRoot.SetActive(false);
            if (_bullet != null)
            {
                Destroy(_bullet.gameObject);
            }
        }

        private void OnTouch(PlayerTouchedEvent touchedEvent)
        {
            if (isPaused)
            {
                return;
            }

            if (_playerState == PlayerState.Cooldown)
            {
                return;
            }

            if (IsAnyButtonTouched(touchedEvent.TouchPosition))
            {
                return;
            }

            if (_playerState == PlayerState.NotSelected)
            {
                LevelInfo.BulletData bulletData = _bulletService.GetBulletData();
                GameObject b = Instantiate(bulletData.Prefab, _rootContainer.transform);
                b.transform.position = _rootContainer.transform.position;
                _bullet = b.GetComponent<RightBodyBullet>();
                _bullet.Init(bulletData);
                _playerState = PlayerState.SelectDirection;
                _arrowRoot.SetActive(true);
                SelectDirection();
                return;
            }

            if (_playerState == PlayerState.SelectDirection)
            {
                _rotationSequence.Kill();
                _playerState = PlayerState.SelectPower;
                SelectPower();
                return;
            }

            if (_playerState == PlayerState.SelectPower)
            {
                _powerSequence.Kill();
                _arrowRoot.SetActive(false);
                _progress.fillAmount = 0;
                _playerState = PlayerState.Cooldown;
                Observable.Timer(TimeSpan.FromSeconds(3))
                    .Subscribe(_ => { _playerState = PlayerState.NotSelected; });

                Fire();
            }
        }

        private void Fire()
        {
            var rotationY = _arrowRoot.transform.rotation.eulerAngles.y;
            _bullet.Launch(new BulletShootModel(0, _angleY, 90 - rotationY, _power, _rootContainer.transform.position));
            if (_fireSound == null || _fireSound.clip == null)
            {
                return;
            }
            _fireSound.Play();
        }

        private void SelectDirection()
        {
            _rotationSequence.Kill();
            _rotationSequence = DOTween.Sequence();
            _rotationSequence.Append(_arrowRoot.transform.DORotate(new Vector3(0, _leftMaxAngle, 0),
                _timeSelectDirection));
            _rotationSequence.Append(_arrowRoot.transform.DORotate(new Vector3(0, _rightMaxAngle, 0),
                _timeSelectDirection));
            _rotationSequence.AppendCallback(SelectDirection);
        }

        private void SelectPower()
        {
            _powerSequence.Kill();
            _powerSequence = DOTween.Sequence();
            var feelTween = DOVirtual.Float(0, 1, _timeSelectPower, (x) =>
            {
                _angleY = x * 60;
                _progress.fillAmount = x;
                _power = POWER_COEF * x;
            });
            var feelOutTween = DOVirtual.Float(1, 0, _timeSelectPower, (x) =>
            {
                _angleY = x * 60;
                _progress.fillAmount = x;
                _power = POWER_COEF * x;
            });
            _powerSequence.Append(feelTween);
            _powerSequence.Append(feelOutTween);
            _powerSequence.AppendCallback(SelectPower);
        }

        public void SetParams(float timeSelectDirection, float timeSelectPower, float leftMaxAngle, float rightMaxAngle)
        {
            _timeSelectDirection = timeSelectDirection;
            _timeSelectPower = timeSelectPower;
            _leftMaxAngle = leftMaxAngle;
            _rightMaxAngle = rightMaxAngle;
        }

        private void OnDestroy()
        {
            _powerSequence?.Kill();
            _powerSequence = null;
            _rotationSequence?.Kill();
            _rotationSequence = null;
            _gameContext.EventDispatcher.RemoveListener<UiPauseGameEvent>(UiPauseGameEvent.PAUSEGAME_EVENT,
                OnPauseEvent);
            _gameContext.EventDispatcher.RemoveListener<PlayerTouchedEvent>(PlayerTouchedEvent.TOUCH_EVENT, OnTouch);
            _gameContext.EventDispatcher.RemoveListener<UiGameplayEvent>(UiGameplayEvent.GAMEPLAY_EVENT,
                OnGamePlayEvent);
        }
    }
}