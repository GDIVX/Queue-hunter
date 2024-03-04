using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using System.Collections;
using UnityEngine;


namespace Assets.Scripts.Engine.ECS.Common
{
    [System.Serializable]
    public class RotationComponent : DataComponent
    {
        [SerializeField] Vector3 _rotation;

        public Vector3 Rotation
        {
            get => _rotation;
            set
            {
                SafeSet<Vector3>(ref _rotation, value);
            }
        }

        public override IComponent Instantiate()
        {
            RotationComponent component = new RotationComponent()
            {
                Rotation = _rotation
            };

            return component;
        }
    }
}