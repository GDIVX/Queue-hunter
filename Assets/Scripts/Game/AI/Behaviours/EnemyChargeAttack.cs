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
        [SerializeField, BoxGroup("Settings")] private float chargeOvershootDistance;
        [SerializeField, BoxGroup("Settings")] private float coolDown, windUp;
        [SerializeField, BoxGroup("Settings")] private float attackDamage;
        [SerializeField, BoxGroup("Settings")] private float speed;
        [SerializeField, BoxGroup("Settings")] private float chargeDistanceToTargetTolerance = 1.5f;

        public UnityEvent onPreparingToCharge;
        public UnityEvent<string, bool> onChargeStart, onChargeEnd;

        private ITarget _target;
        private Vector3 _destination;
        private Vector3 _velocity;
        private bool _hasAttacked;
        [ShowInInspector] private ChargeState _currentState;

        public ChargeState CurrentState => _currentState;

        public enum ChargeState
        {
            Charging,
            Seeking,
            Recovering
        }

        private void OnValidate()
        {
            movementController ??= GetComponent<EnemyMovement>();
            targeting ??= GetComponent<EnemyTargeting>();
            rigidbody ??= GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            targeting.OnTargetFound += OnTargetFound;
            _currentState = ChargeState.Seeking;
        }

        private void Update()
        {
            if (_currentState != ChargeState.Charging) return;
            HandleChargeMovement();
        }

        private void HandleChargeMovement()
        {
            var distance = Vector3.Distance(transform.position, _destination);
            if (distance <= chargeDistanceToTargetTolerance)
            {
                EndCharge();
                return;
            }

            var direction = (_destination - transform.position).normalized;
            _velocity = direction * speed;
            rigidbody.MovePosition(transform.position + _velocity * Time.deltaTime);
        }

        private void OnTargetFound(ITarget target)
        {
            if (_target != null) return;
            if (_currentState != ChargeState.Seeking) return;

            if (!CanCharge(target)) return;
            _target = target;
            StartCoroutine(HandleCooldown(target));
        }

        private void ChargeAt(ITarget target)
        {
            if (_currentState == ChargeState.Charging)
            {
                Debug.LogError($"{name} is trying to start a charge while already in charge state.");
                return;
            }

            _destination = GetChargeDestination(target);
            _hasAttacked = false;
            StartCharge(target);
        }

        private Vector3 GetChargeDestination(ITarget target)
        {
            var direction = (target.Position - transform.position).normalized;
            var distanceToTargetWithOvershoot =
                Vector3.Distance(transform.position, target.Position) + chargeOvershootDistance;
            var distance = Mathf.Min(maxChargeDistance, distanceToTargetWithOvershoot);
            var destination = transform.position + direction *
                distance;
            return destination;
        }

        private void OnCollisionEnter(Collision other)
        {
            HandleCollision(other.collider);
        }

        private void HandleCollision(Collider other)
        {
            if (_currentState != ChargeState.Charging || _target == null) return;

            if (other.gameObject != _target.GameObject)
            {
                return;
            }

            HandleDamage(_target.Damageable);
            EndCharge();
        }

        private void HandleDamage(IDamageable targetDamageable)
        {
            if (_hasAttacked) return;
            targetDamageable.HandleDamage(attackDamage);
            _hasAttacked = true;
        }

        private IEnumerator HandleCooldown(ITarget target)
        {
            onPreparingToCharge?.Invoke();
            yield return new WaitForSeconds(coolDown);
            ChargeAt(target);
        }

        private void StartCharge(ITarget target)
        {
            _currentState = ChargeState.Charging;
            _target = target;
            onChargeStart?.Invoke("isLockedRunning", true);
            movementController.SetMovementAllowed(false);
        }

        private void EndCharge()
        {
            _target = null;
            _currentState = ChargeState.Recovering;
            onChargeEnd?.Invoke("isLockedRunning", false);
            StartCoroutine(HandleWindup());
        }

        private IEnumerator HandleWindup()
        {
            yield return new WaitForSeconds(windUp);
            _currentState = ChargeState.Seeking;
            movementController.SetMovementAllowed(true);
        }

        private bool CanCharge(ITarget target)
        {
            if (_currentState is ChargeState.Charging or ChargeState.Recovering) return false;
            var distanceToTarget = Vector3.Distance(transform.position, target.Position);
            return distanceToTarget <= distanceToTargetRange.Max && distanceToTarget >= distanceToTargetRange.Min;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxChargeDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, distanceToTargetRange.Min);
            Gizmos.DrawWireSphere(transform.position, distanceToTargetRange.Max);

            if (_currentState != ChargeState.Charging) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _destination);
        }
    }
}