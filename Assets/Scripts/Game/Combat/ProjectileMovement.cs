using Combat;
using Game.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat
{
    public sealed class ProjectileMovement : MonoBehaviour, IInit<ProjectileModel>
    {
        [SerializeField] private float speed;
        [SerializeField] private float windUpTime;

        public Vector3 Direction { get; set; }

        private void Start()
        {
            StartCoroutine(ProjectileWindUp());
        }

        public void Initialize(float speed)
        {
            this.speed = speed;
        }

        private void FixedUpdate()
        {
            if (windUpTime == 0)
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

        private IEnumerator ProjectileWindUp()
        {
            yield return new WaitForSeconds(windUpTime);
            windUpTime = 0;
        }
    }
}