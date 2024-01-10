using System;
using Game.Model;
using Game.UFO.Model;
using UnityEngine;

namespace Game
{
    public class RightBodyBullet : MonoBehaviour, ILaunch
    {
        [SerializeField] private Rigidbody _rtiRigidbody;
        private BulletShootModel _bulletShootModel;
        private LevelInfo.BulletData _bulletData;
        
        [Header("Sound")] 
        [SerializeField] private AudioSource _contactFloorSound;

        private void FixedUpdate()
        {
            if(_bulletShootModel != null)
                if(Vector3.Distance(_bulletShootModel.StartShootPosition, transform.position) > 1000)
                    Destroy(gameObject);
        }

        private Vector3 CalculateVelocity(BulletShootModel bulletShootModel)
        {
            float radianAngleY = bulletShootModel.AngleY * Mathf.Deg2Rad;
            float radianAngleZ = bulletShootModel.AngleZ * Mathf.Deg2Rad;

            float vx = bulletShootModel.Force * Mathf.Cos(radianAngleY) * Mathf.Cos(radianAngleZ);
            float vy = bulletShootModel.Force * Mathf.Sin(radianAngleY);
            float vz = bulletShootModel.Force * Mathf.Cos(radianAngleY) * Mathf.Sin(radianAngleZ);

            return new Vector3(vx, vy, vz);
        }

        public void Launch(BulletShootModel bulletShootModel)
        {
            _bulletShootModel = bulletShootModel;
            _rtiRigidbody.isKinematic = false;
            _rtiRigidbody.velocity = CalculateVelocity(bulletShootModel);
        }

        private void OnTriggerEnter(Collider other)
        {
            ITargetModel targetModel = other.GetComponentInChildren<ITargetModel>();
            if (targetModel == null)
            {
                if (_contactFloorSound == null || _contactFloorSound.clip ==null)
                {
                    return;
                }
                _contactFloorSound.Play();
                return;
            }
            targetModel.Contact(_bulletShootModel, _bulletData);
            Destroy(gameObject);
        }

        public void Init(LevelInfo.BulletData bulletData)
        {
            _bulletData = bulletData;
        }
    }
}