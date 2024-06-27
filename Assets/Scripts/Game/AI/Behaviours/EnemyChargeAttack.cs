using System.Collections;
using AI;
using Combat;
using Game.Combat;
using Game.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Game.AI.Behaviours
{
    [RequireComponent(typeof(Collider))]
    public class EnemyChargeAttack : MonoBehaviour
    {
        [SerializeField, BoxGroup("Dependencies")]
        private EnemyMovement movementController;

        [SerializeField, BoxGroup("Dependencies")]
        private new Rigidbody rigidbody;

        [SerializeField, BoxGroup("Dependencies")]
        private EnemyTargeting targeting;

        [SerializeField, BoxGroup("Settings"),
         Tooltip("The range to look for target. Targets would only be visible within this range")]
        private MinMaxFloat distanceToTargetRange;

        [SerializeField, BoxGroup("Settings")] private float maxChargeDistance;
        [SerializeField, BoxGroup("Settings")] private float coolDown;
        [SerializeField, BoxGroup("Settings")] private float attackDamage;
        [SerializeField, BoxGroup("Settings")] private float speed;

        private ITarget _target;

        public UnityEvent onPreparingToCharge, onChargeStart, onChargeEnd;

        private bool _isCharging = false;
        private Vector3 _destination;

        private void OnValidate()
        {
            movementController ??= GetComponent<EnemyMovement>();
            targeting ??= GetComponent<EnemyTargeting>();
            rigidbody ??= GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            targeting.OnTargetFound += OnTargetFound;
        }

        private void Update()
        {
            if (!_isCharging) return;
            HandleChargeMovement();
        }

        private void HandleChargeMovement()
        {
            // If we reached the destination - we can no longer charge
            if (Vector3.Distance(transform.position, _destination) <= 0.1f) // Added a small tolerance
            {
                EndCharge();
                return;
            }

            var direction = (_destination - transform.position).normalized;
            rigidbody.velocity = direction * speed;
        }

        private void OnTargetFound(ITarget target)
        {
            if (!CanCharge(target)) return;

            _target = target;
            StartCoroutine(HandleCooldown(target));
        }

        private void ChargeAt(ITarget target)
        {
            _destination = GetChargeDestination(target);
            StartCharge(target);
        }

        private Vector3 GetChargeDestination(ITarget target)
        {
            var direction = (target.Position - transform.position).normalized;
            var destination = transform.position + direction *
                Mathf.Min(maxChargeDistance, Vector3.Distance(transform.position, target.Position));
            return destination;
        }

        private void OnCollisionEnter(Collision other)
        {
            HandleCollision(other.collider);
        }

        private void HandleCollision(Collider other)
        {
            if (!_isCharging) return;

            if (_target == null) return;

            if (other.gameObject != _target.GameObject)
            {
                EndCharge();
                return;
            }

            HandleDamage(_target.Damageable);
            EndCharge();
        }

        private void HandleDamage(IDamageable targetDamageable)
        {
            targetDamageable.HandleDamage(attackDamage);
        }

        private IEnumerator HandleCooldown(ITarget target)
        {
            onPreparingToCharge?.Invoke();
            yield return new WaitForSeconds(coolDown);
            ChargeAt(target);
        }

        private void StartCharge(ITarget target)
        {
            _isCharging = true;
            _target = target;
            onChargeStart?.Invoke();
            movementController.SetMovementAllowed(false);
        }

        private void EndCharge()
        {
            _isCharging = false;
            _target = null;
            onChargeEnd?.Invoke();
            rigidbody.velocity = Vector3.zero;
            movementController.SetMovementAllowed(true);
        }

        private bool CanCharge(ITarget target)
        {
            if (_isCharging) return false;

            var distanceToTarget = Vector3.Distance(transform.position, target.Position);
            var isWithinRange = distanceToTarget <= distanceToTargetRange.Max &&
                                distanceToTarget >= distanceToTargetRange.Min;
            return isWithinRange;
        }
    }
}