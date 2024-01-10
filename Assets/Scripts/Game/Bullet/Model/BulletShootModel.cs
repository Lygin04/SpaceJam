using UnityEngine;

namespace Game.Model
{
    public class BulletShootModel
    {
        private readonly float _angleX;
        private readonly float _angleY;
        private readonly float _angleZ;
        private readonly float _force;
        private readonly Vector3 _startShootPosition;

        public BulletShootModel(float angleX, float angleY, float angleZ, float force, Vector3 startShootPosition)
        {
            _angleX = angleX;
            _angleY = angleY;
            _angleZ = angleZ;
            _force = force;
            _startShootPosition = startShootPosition;
        }

        public Vector3 StartShootPosition => _startShootPosition;


        public float Force => _force;

        public float AngleX => _angleX;

        public float AngleY => _angleY;

        public float AngleZ => _angleZ;
    }
}