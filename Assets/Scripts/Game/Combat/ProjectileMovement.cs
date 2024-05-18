using Combat;
using Game.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat
{
    public sealed class ProjectileMovement : MonoBehaviour, IInit<ProjectileModel>
    {
        [SerializeField] private float speed;

        public Vector3 Direction { get; set; }

        public void Initialize(float speed)
        {
            this.speed = speed;
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        public void HandleMovement()
        {
            var direction = transform.forward;
            Vector3 translation = direction * (speed * Time.fixedDeltaTime);
            transform.position += translation;
        }

        public void Init(ProjectileModel input)
        {
            speed = input.Speed;
        }
    }
}