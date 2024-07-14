using Combat;
using Game.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Combat
{
    public sealed class ProjectileMovement : MonoBehaviour, IInit<ProjectileModel>
    {
        [SerializeField] private float speed;
        [SerializeField] private float windUpTime;

        private bool canMove;

        public bool CanMove
        {
            get => canMove;
            set => canMove = value;
        }

        public UnityEvent onMarbleShot;
        public UnityEvent onMarbleShotEnd;

        public Vector3 Direction { get; set; }

        private void OnEnable()
        {
            StartCoroutine(ProjectileWindUp());
            
        }

        public void Initialize(float speed)
        {
            this.speed = speed;
        }

        private void FixedUpdate()
        {
            if (canMove)
                HandleMovement();
        }

        public void HandleMovement()
        {
            onMarbleShotEnd?.Invoke();
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
            onMarbleShot?.Invoke();
            canMove = false;
            yield return new WaitForSeconds(windUpTime);
            canMove = true;
        }

        public void CancelMovement()
        {
            canMove = false;
        }
    }
}