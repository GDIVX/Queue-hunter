using Assets.Scripts.Core.ECS;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Game.Movement
{
    [CreateAssetMenu(fileName = "Movement", menuName = "Game/Movement/MovementParams")]
    public class MovementParams : DataComponent
    {
        [ShowInInspector]
        private float speed;

        [ShowInInspector]
        private Vector3 lastDir;

        public Vector3 LastDir
        {
            get => lastDir; 
            set
            {
                SafeSet(ref lastDir, value);
            }
        }

        public float Speed
        {
            get => speed;
            set
            {
                SafeSet(ref speed, value);
            }
        }
    }
}