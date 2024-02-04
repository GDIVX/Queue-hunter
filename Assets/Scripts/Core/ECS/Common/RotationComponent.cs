using System.Collections;
using UnityEngine;


namespace Assets.Scripts.Engine.ECS.Common
{
    [CreateAssetMenu(fileName = "RotationComponent", menuName = "ECS/Components/RotationComponent")]
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
    }
}